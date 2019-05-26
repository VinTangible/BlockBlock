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
        }
    }
}
