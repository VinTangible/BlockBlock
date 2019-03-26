using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector2 offset;

    // Sets the sorting layer of the block's sprites based on passed in layer name
    public void SetSortingLayer(string layerName)
    {   
        foreach (Transform child in transform)
        {
            SpriteRenderer sprite = child.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = SortingLayer.NameToID(layerName);
            sprite.sortingLayerName = layerName;
        }
    }

    // Mouse Functions
    void OnMouseDown()
    {
        // Calculate offset of the mouse position and starting position of parent when drag begins
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // Set layer of game object to "Up"
        SetSortingLayer(GameManager.UP_LAYER);
    }

    void OnMouseDrag()
    {
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = newPosition - offset;
    }

    void OnMouseUp()
    {
        // When mouse is released, set the piece down and set sorting layer
        SetSortingLayer(GameManager.DOWN_LAYER);
        GameManager.instance.DropPiece(gameObject);
    }
}
