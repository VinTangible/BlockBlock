using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 10;

    public static GameManager gameManager = null;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
        }
        else if(gameManager != this)
        {
            Destroy(gameObject);
        }
    }

    public void SnapToGrid(Block block)
    {
        Debug.Log("Snapping to grid...");

        if(IsInsideGrid(block))
        {
            foreach(Transform blockPiece in block.transform)
            {
                Vector2 roundedPosition = Round(block.transform.position);

                block.transform.position = roundedPosition;

                blockPiece.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else
        {
            // bring block back to original spawn position
        }
    }

    // checks if a block is inside the grid by iterating through
    // all of its individual pieces and checking their positions
    public bool IsInsideGrid(Block block)
    {
        foreach(Transform blockPiece in block.transform)
        {
            if((int) blockPiece.position.x < 0 ||
               (int) blockPiece.position.x >= gridWidth ||
               (int) blockPiece.position.y < 0 || 
               (int) blockPiece.position.y >= gridHeight)
            {
                return false;
            }
        }

        return true;
    }

    // Rounds the x and y positions to the nearest integer
    public Vector2 Round(Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }
}
