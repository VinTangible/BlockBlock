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

    public void SpawnNextBlock(Vector2 startingPosition)
    {
        GameObject nextBlock = (GameObject)Instantiate(
            Resources.Load(GetRandomBlock(), typeof(GameObject)),
                startingPosition, Quaternion.identity);
        
        RotateBlock(nextBlock.GetComponentInParent<Block>());
    }

    public bool SnapToGrid(Block block)
    {
        if(IsInsideGrid(block) && IsValidPlacement(block))
        {
            foreach(Transform blockPiece in block.transform)
            {
                // round the block's current position to the nearest integer
                // to have it snap to the closest available grid space
                Vector2 roundedPosition = Round(block.transform.position);

                // set the block's new position; this is where the snapping actually occurs
                block.transform.position = roundedPosition;

                // update the grid to fill in the now occupied space
                grid[(int) roundedPosition.x, (int) roundedPosition.y] = block.transform;

                // disabling the BoxCollider2D component will disable dragging
                blockPiece.GetComponent<BoxCollider2D>().enabled = false;

                // disabling the Block component in the parent will disable Block's script.
                // this prevents multiple blocks from spawning after this current block is placed
                blockPiece.GetComponentInParent<Block>().enabled = false;
            }

            return true;
        }

        // else, bring block back to original spawn position
        block.transform.position = block.startingPosition;
        return false;
    }

    // checks if a block is inside the grid by iterating through
    // all of its individual pieces and checking their positions
    public bool IsInsideGrid(Block block)
    {
        foreach(Transform blockPiece in block.transform)
        {
            Vector2 roundedPosition = Round(blockPiece.transform.position);

            if((int) roundedPosition.x < 0 ||
               (int) roundedPosition.x > gridWidth - 1 ||
               (int) roundedPosition.y < 0 || 
               (int) roundedPosition.y > gridHeight - 1)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsValidPlacement(Block block)
    {
        foreach(Transform blockPiece in block.transform)
        {
            Vector2 roundedPiecePos = Round(blockPiece.position);

            if(grid[(int) roundedPiecePos.x, (int) roundedPiecePos.y] != null)
            {
                // space is occupied
                return false;
            }
        }

        // we've checked all the block pieces' current position relative to the grid.
        // those spaces are empty, so we're okay to place it down
        return true;
    }

    // Rounds the x and y positions to the nearest integer
    public Vector2 Round(Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }

    // Rotates the block component in increments of 90, 180, or 270 degrees.
    // Squares don't need to be rotated and the long pieces only have to be rotated 90 degrees.
    // The el pieces are the ones that have full rotation allowed.
    void RotateBlock(Block block)
    {
        if(block.allowRotation)
        {
            if(block.limitRotation)
            {
                // this is for the long pieces; rotate 90 degrees only
                block.transform.Rotate(new Vector3(0, 0, GetRandomRotation(true)));
            }
            else
            {
                // this is for the el pieces, they have full rotation
                block.transform.Rotate(new Vector3(0, 0, GetRandomRotation(false)));
            }
        }
    }

    string GetRandomBlock()
    {
        string prefabPath = "Prefabs\\Blocks\\";

        string[] blocks = {"El2", "El3", "Long2", "Long3", "Long4", "Long5", 
            "Square1", "Square2", "Square3"};

        int randomBlock = Random.Range(0, 9);

        return prefabPath + blocks[randomBlock];
    }

    // Chooses a random starting position for each block.
    // All of the starting positions have the same y value,
    // so we're choosing pre-determined x values.
    // Vector2 GetRandomStartPosition()
    // {
    //     float[] xPositions = {-3.5f, 1.5f, 5f, 11f};

    //     int randomXPosition = Random.Range(0, 4);

    //     return new Vector2(xPositions[randomXPosition], -2.5f);
    // }

    int GetRandomRotation(bool isLongPiece)
    {
        int[] rotations = {0, 90, 180, 270};
        int randomRotation = 0;

        if(isLongPiece)
        {
            randomRotation = Random.Range(0, 2);
        }
        else
        {
            randomRotation = Random.Range(0, 4);
        }

        return rotations[randomRotation];
    }
}
