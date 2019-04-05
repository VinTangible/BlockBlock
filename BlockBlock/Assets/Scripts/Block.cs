using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector2 offset;
    private Vector2 spawnPosition;

    // Sets the sorting layer of the block's sprites based on passed in layer name
    private void SetSortingLayer(string layerName)
    {   
        foreach (Transform child in transform)
        {
            SpriteRenderer sprite = child.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = SortingLayer.NameToID(layerName);
            sprite.sortingLayerName = layerName;
        }
    }

    public void Init(Vector2 pos)
    {
        // Get adjusted spawn position used to center the block at spawn
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        transform.localScale = new Vector2((float)0.75, (float)0.75);

        // Apply (.5, .5) offset because (.5, .5) of the block sticks out
        spawnPosition = pos - collider.size/2 + new Vector2(.5f, .5f);
        transform.position = spawnPosition;
    }

    public void ResetPosition()
    {
        if (spawnPosition != null)
        {
            transform.localScale = new Vector2((float)0.75, (float)0.75);
            transform.position = spawnPosition;
        }
        else
        {
            Debug.Log("Spawn position was not set.");
        }
    }

    // Mouse Functions
    void OnMouseDown()
    {
        // Calculate offset of the mouse position and starting position of parent when drag begins
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        transform.localScale = new Vector2(1, 1);

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
        GameManager.instance.DropPiece(this);
    }
}
