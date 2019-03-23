using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 10;

    public static GameManager gameManager = null;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    public List<int> rowsToDelete;
    public List<int> columnsToDelete;

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

    // spawns a new block in the starting position of the recently placed block
    public void SpawnNextBlock(Vector2 startingPosition)
    {
        GameObject nextBlock = (GameObject)Instantiate(
            Resources.Load(GetRandomBlock(), typeof(GameObject)),
                startingPosition, Quaternion.identity);
        
        RotateBlock(nextBlock.GetComponentInParent<Block>());
    }

    public void ClearRowsAndColumns()
    {
        foreach(int row in rowsToDelete)
        {
            ClearRowAt(row);
        }

        foreach(int column in columnsToDelete)
        {
            ClearColumnAt(column);
        }

        rowsToDelete.Clear();
        columnsToDelete.Clear();
    }

    public void CheckRows()
    {
        for(int y = 0; y < gridHeight; ++y)
        {
            for(int x = 0; x < gridWidth; ++x)
            {
                if(grid[x, y] == null)
                {
                    // we've found an empty space
                    break;
                }

                if(x == gridWidth - 1)
                {
                    // mark this row for deletion
                    rowsToDelete.Add(y);
                }
            }
        }
    }

    public void CheckColumns()
    {
        for(int x = 0; x < gridWidth; ++x)
        {
            for(int y = 0; y < gridHeight; ++y)
            {
                if(grid[x, y] == null)
                {
                    // we've found an empty space
                    break;
                }

                if(y == gridHeight - 1)
                {
                    // mark this column for deletion
                    columnsToDelete.Add(x);
                }
            }
        }
    }
    
    public void ClearRowAt(int y)
    {
        for(int x = 0; x < gridWidth; ++x)
        {
            if(grid[x, y] != null)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
    }

    public void ClearColumnAt(int x)
    {
        for(int y = 0; y < gridHeight; ++y)
        {
            if(grid[x, y] != null)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
    }

    // upon letting go of the block, this function will round off
    // the block's position to the nearest integer to snap it to
    // the closest available grid space
    public bool SnapToGrid(Block block)
    {
        if(IsInsideGrid(block) && IsValidPlacement(block))
        {
            foreach(Transform blockPiece in block.transform)
            {
                // round the block's current position to the nearest integer
                // to have it snap to the closest available grid space
                Vector2 roundedPosition = Round(block.transform.position);
                Vector2 roundedPiecePos = Round(blockPiece.transform.position);

                // set the block's new position; this is where the snapping actually occurs
                block.transform.position = roundedPosition;

                // update the grid to fill in the now occupied space
                grid[(int) roundedPiecePos.x, (int) roundedPiecePos.y] = blockPiece;

                // disabling the BoxCollider2D component will disable dragging
                blockPiece.GetComponent<BoxCollider2D>().enabled = false;
            }

            // disabling the Block component will disable Block's script.
            // this prevents multiple blocks from spawning after this current block is placed
            block.enabled = false;

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

    // checks for occupied grid spaces of the current block's position
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

    // retrieves a random block prefab from the resources folder
    string GetRandomBlock()
    {
        string prefabPath = "Prefabs\\Blocks\\";

        string[] blocks = {"El2", "El3", "Long2", "Long3", "Long4", "Long5", 
            "Square1", "Square2", "Square3"};

        int randomBlock = Random.Range(0, 9);

        return prefabPath + blocks[randomBlock];
    }

    // returns a random rotation in increments of 90
    int GetRandomRotation(bool isLongPiece)
    {
        int[] rotations = {0, 90, 180, 270};
        int randomRotation = 0;

        if(isLongPiece)
        {
            // long pieces only need to rotate 90 degrees
            randomRotation = Random.Range(0, 2);
        }
        else
        {
            // the el pieces are allowed to freely rotate
            randomRotation = Random.Range(0, 4);
        }

        return rotations[randomRotation];
    }
}
