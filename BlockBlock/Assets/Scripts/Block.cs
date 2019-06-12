using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector2 startingPosition;

    // these variables are set in Unity
    public bool allowRotation;
    public bool limitRotation;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition.x = this.transform.position.x;
        startingPosition.y = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // upon dragging and letting go of the block,
        // have it automatically snap to the nearest valid spot on the grid
        if(Input.GetMouseButtonUp(0))
        {
            GameManager.gameManager.SnapToGrid(this);
            SetSortingLayer(false);
        }
    }

    // Sets the sorting layer of the block's sprites based on passed in layer name
    public void SetSortingLayer(bool up)
    {   
        int change = up ? 1 : 0;
        foreach (Transform child in transform)
        {
            SpriteRenderer sprite = child.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = change;
        }
    }
}
