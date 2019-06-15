using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static float SMALL_SCALE = .6f;
    
    // Limit numRotations to be values from 0 to 3
    [Range(0,3)]
    public int numRotations;

    private Vector2 offset;
    private Vector2 spawnPosition;
    private int[] rotations = new int[] {0, 90, 180, 270};

    // Initializing happens here
    void Awake()
    {
        // Set position when block is initialized
        spawnPosition = transform.position;

        // Scale and rotate when block is rendered
        transform.localScale = new Vector2(SMALL_SCALE,SMALL_SCALE);
        RandomRotate();
    }

    void Start()
    {
        
    }

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

    private void RandomRotate()
    {
        transform.Rotate(0,0, rotations[Random.Range(0, numRotations + 1)]);
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

    // Public Functions

    public void ScaleToUpLayer()
    {
        transform.localScale = new Vector2(1, 1);
    }

    // Resets block back to its original spawn position
    public void ResetPosition()
    {
        if (spawnPosition != null)
        {
            transform.position = spawnPosition;
            transform.localScale = new Vector2(SMALL_SCALE,SMALL_SCALE);
        }
        else
        {
            Debug.Log("Spawn position was not set.");
        }
    }
}
