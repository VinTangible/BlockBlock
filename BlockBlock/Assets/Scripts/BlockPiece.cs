using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPiece : MonoBehaviour
{
    private Vector2 offset;

    //When Mouse is clicked
    void OnMouseDown() {
        // Calculate offset of the mouse position and starting position of parent when drag begins
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position;
        transform.parent.localScale = new Vector3( 1, 1, 1);
    }

    //Dragging the block pieces
    void OnMouseDrag() {
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.parent.position = newPosition - offset;
    }
}
