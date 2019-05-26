using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int GRID_WIDTH = 10;
    public static int GRID_HEIGHT = 10;
    public static int NUM_SPAWN = 3;

    public static int numDropped = 0;

    public static GameManager gameManager = null;

    public static Transform[,] grid = new Transform[GRID_WIDTH, GRID_HEIGHT];

    public List<int> rowsToDelete;
    public List<int> columnsToDelete;

    private Vector2[] spawnPositions = new Vector2[NUM_SPAWN];
    private List<Block> blocksToPlace = new List<Block>();

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

        InitializeStartingPosititions();
        SpawnBlocks();
    }

    // spawns a new set of blocks after all blocks have been placed
    public void SpawnBlocks()
    {
        for(int i = 0; i < NUM_SPAWN; ++i)
        {
            GameObject block = GetRandomBlock();
            GameObject nextBlock = (GameObject)Instantiate(block, spawnPositions[i], Quaternion.identity);
        
            RotateBlock(nextBlock.GetComponentInParent<Block>());

            blocksToPlace.Add(nextBlock.GetComponentInParent<Block>());
        }

        numDropped = 0;
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
        for(int y = 0; y < GRID_HEIGHT; ++y)
        {
            for(int x = 0; x < GRID_WIDTH; ++x)
            {
                if(grid[x, y] == null)
                {
                    // we've found an empty space
                    break;
                }

                if(x == GRID_WIDTH - 1)
                {
                    // mark this row for deletion
                    rowsToDelete.Add(y);
                }
            }
        }
    }

    public void CheckColumns()
    {
        for(int x = 0; x < GRID_WIDTH; ++x)
        {
            for(int y = 0; y < GRID_HEIGHT; ++y)
            {
                if(grid[x, y] == null)
                {
                    // we've found an empty space
                    break;
                }

                if(y == GRID_HEIGHT - 1)
                {
                    // mark this column for deletion
                    columnsToDelete.Add(x);
                }
            }
        }
    }
    
    public void ClearRowAt(int y)
    {
        for(int x = 0; x < GRID_WIDTH; ++x)
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
        for(int y = 0; y < GRID_HEIGHT; ++y)
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
    public void SnapToGrid(Block block)
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

            ++numDropped;
            Debug.Log("num dropped: " + numDropped);

            CheckRows();
            CheckColumns();
            ClearRowsAndColumns();
            
            if(numDropped == NUM_SPAWN)
            {
                Debug.Log("Clearing list of blocks to place");
                blocksToPlace.Clear();
                Debug.Log("Spawning new blocks!");
                SpawnBlocks();
            }

            CheckForGameOver();
        }
        else
        {
            // else, bring block back to original spawn position
            block.transform.position = block.startingPosition;
        }
    }

    // checks if a block is inside the grid by iterating through
    // all of its individual pieces and checking their positions
    public bool IsInsideGrid(Block block)
    {
        foreach(Transform blockPiece in block.transform)
        {
            Vector2 roundedPosition = Round(blockPiece.transform.position);

            if((int) roundedPosition.x < 0 ||
               (int) roundedPosition.x > GRID_WIDTH - 1 ||
               (int) roundedPosition.y < 0 || 
               (int) roundedPosition.y > GRID_HEIGHT - 1)
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

    private bool IsValidDrop(Block block)
    {
        for(int x = 0; x < GRID_WIDTH; ++x)
        {
            for(int y = 0; y < GRID_HEIGHT; ++y)
            {
                Vector2 originalPosition = block.transform.position;
                block.transform.position = new Vector2(x, y);

                if(IsInsideGrid(block) && IsValidPlacement(block))
                {
                    block.transform.position = originalPosition;
                    return true;
                }
                else
                {
                    block.transform.position = originalPosition;
                }
            }
        }

        return false;
    }

    // TODO: make this dynamic and not hardcoded
    private void InitializeStartingPosititions()
    {
        spawnPositions[0] = new Vector2(-5.5f, -4.5f);
        spawnPositions[1] = new Vector2(4.0f, -4.5f);
        spawnPositions[2] = new Vector2(14f, -4.5f);
    }

    public void CheckForGameOver()
    {
        foreach(Block block in blocksToPlace)
        {
            foreach(Transform blockPiece in block.transform)
            {
                // **********************************************************************
                // TODO: make one BoxCollider for the entire block rather than each
                // child have its own BoxCollider
                // **********************************************************************
                if(blockPiece.GetComponent<BoxCollider2D>().enabled)
                {
                    if(IsValidDrop(block))
                    {
                        return;
                    }
                }
            }
        }

        Debug.Log("Game Over");

        // load Game Over screen
        SceneManager.LoadScene("GameOver");
    }
    
    // Rounds the x and y positions to the nearest integer
    private Vector2 Round(Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }

    // Rotates the block component in increments of 90, 180, or 270 degrees.
    // Squares don't need to be rotated and the long pieces only have to be rotated 90 degrees.
    // The el pieces are the ones that have full rotation allowed.
    private void RotateBlock(Block block)
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

    // TODO: implement this correctly to not hardcode starting positions
    // private Vector2 GetStartingPosition(GameObject block)
    // {
    //     BoxCollider2D box = block.GetComponent<BoxCollider2D>();

    //     return box.transform.position;
    // }

    // retrieves a random block prefab from the resources folder
    private GameObject GetRandomBlock()
    {
        string prefabPath = "Prefabs\\Blocks\\";

        GameObject[] blocks = Resources.LoadAll<GameObject>(prefabPath);

        int randomBlock = Random.Range(0, blocks.Length);

        return blocks[randomBlock];
    }

    // returns a random rotation in increments of 90
    private int GetRandomRotation(bool isLongPiece)
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
