using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector2 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition.x = this.transform.position.x;
        startingPosition.y = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(GameManager.gameManager.SnapToGrid(this))
            {
                // if block was successfully placed, spawn new block
                GameManager.gameManager.SpawnNextBlock(startingPosition);
            }
        }
    }
}
