using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float SCALE = .6f; // Used for scaling down blocks when in their spawn positions
    
    // Limit numRotations to be values from 0 to 3
    [Range(0,3)]
    public int numRotations;

    private Vector2 mouseClickOffset; // Offset of mouse click from the block's origin
    private Vector2 spawnPosition;

    // Initializing happens here
    void Awake()
    {
        // Set position when block is initialized
        spawnPosition = transform.position;

        // Scale down block when rendered
        ScaleDown();
    }

    // Increments or decrements sorting layer based on passed in bool
    private void SetSortingLayer(bool up)
    {   
        int change = up ? 1 : -1;
        foreach (Transform child in transform)
        {
            SpriteRenderer sprite = child.GetComponent<SpriteRenderer>();
            sprite.sortingOrder += change;
        }
    }

    // Mouse Functions
    void OnMouseDown()
    {
        // Calculate offset of the mouse position and starting position of parent when drag begins
        mouseClickOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        ResetScale();

        // Set layer of game object to "Up"
        SetSortingLayer(true);
    }

    void OnMouseDrag()
    {
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = newPosition - mouseClickOffset;
    }

    void OnMouseUp()
    {
        // When mouse is released, set the piece down and set sorting layer
        SetSortingLayer(false);
        GameManager.instance.DropPiece(this);
    }

    // Public Functions

    public void ResetScale()
    {
        transform.localScale = new Vector2(1, 1);
    }

    public void ScaleDown()
    {
        transform.localScale = new Vector2(SCALE,SCALE);
    }

    // Resets block back to its original spawn position
    public void ResetPosition()
    {
        if (spawnPosition != null)
        {
            transform.position = spawnPosition;
            ScaleDown();
        }
        else
        {
            Debug.Log("Spawn position was not set.");
        }
    }
}
