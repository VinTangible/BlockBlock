using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton GameManager instance
    public static GameManager instance = null;

    public static int GRID_WIDTH = 10;
    public static int GRID_HEIGHT = 10;
    public static int NUM_SPAWN = 3;

    // Name of sorting layers
    public static string UP_LAYER = "Up";
    public static string DOWN_LAYER = "Down";

    int dropCount = 0;

    Block[] blocks;    // Used to store Block prefabs
    Transform pieces;       // Used to store Blocks in Scene Hierarchy

    Transform[,] grid = new Transform[GRID_WIDTH, GRID_HEIGHT];
    HashSet<int> rowsToCheck = new HashSet<int>();
    HashSet<int> colsToCheck = new HashSet<int>();

    Block[] spawnedBlocks = new Block[NUM_SPAWN];
    Vector2[] spawnPositions = new Vector2[NUM_SPAWN];

    // Create the singleton instance when GameManager starts
    void Awake()
    {
        // If singleton does not exist yet, set it
        if (instance == null) {
            instance = this;
        }
        // Otherwise destroy this gameObject being created
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CalculateSpawnPositions();

        // Load block prefabs
        blocks = Resources.LoadAll<Block>("Prefabs/Blocks");

        // Create transform to hold all block pieces
        pieces = new GameObject("Pieces").transform;

        // Spawn inital block
        SpawnBlocks();
    }

    // Drops the Block onto the board if valid
    public void DropPiece(Block block)
    {
        // Round the block's position to align with grid
        block.transform.position = Round(block.transform.position);

        if (IsValidPosition(block))
        {
            // Disable the dropped block
            block.GetComponent<BoxCollider2D>().enabled = false;

            // Increase drop counter
            dropCount++;

            // Add block pieces to the grid
            foreach (Transform blockPiece in block.transform)
            {
                grid[(int)blockPiece.position.x, (int)blockPiece.position.y] = blockPiece;

                // A block was placed in this row/col so mark as needing to check
                rowsToCheck.Add((int)blockPiece.position.y);
                colsToCheck.Add((int)blockPiece.position.x);
            }

            ClearLines();

            // Spawn new blocks if all the blocks have been placed
            if (dropCount == NUM_SPAWN)
            {
                SpawnBlocks();
            }

            // If there are no more valid drops, game over
            if (!HasValidDrop())
            {
                GameOver();
            }
        }
        else
        {
            block.ResetPosition();
        }

    }

    // Private Functions

    // Clears all the full rows/cols

    // Check if there is a valid drop position for any of the spawned blocks
    private bool HasValidDrop()
    {
        foreach (Block block in spawnedBlocks)
        {
            // Only check if the block hasn't been dropped yet
            if (block.GetComponent<BoxCollider2D>().enabled)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    for (int y = 0; y < GRID_HEIGHT; y++)
                    {
                        // Move block to position and check if it is a valid position
                        block.transform.position = new Vector2(x, y);

                        if (IsValidPosition(block))
                        {
                            // Reset block back to its spawn position
                            block.ResetPosition();

                            return true;
                        }
                    }
                }
                
                // Reset block back to its spawn position after checking
                block.ResetPosition();
            }
        }

        return false;
    }

    private void ClearLines()
    {
        // Used to keep track of which rows/cols to clear
        HashSet<int> rowsToClear = new HashSet<int>();
        HashSet<int> colsToClear = new HashSet<int>();

        // Check for full rows
        foreach (int row in rowsToCheck)
        {
            if (IsLineFull(row, true))
            {
                rowsToClear.Add(row);
            }
        }

        // Check for full cols
        foreach(int col in colsToCheck)
        {
            if (IsLineFull(col, false))
            {
                colsToClear.Add(col);
            }
        }

        // Clear rows
        foreach(int row in rowsToClear)
        {
            ClearLine(row, true);
        }

        // Clear cols
        foreach(int col in colsToClear)
        {
            ClearLine(col, false);
        }

        // Clear hashsets used to indicate which rows/cols to check
        rowsToCheck.Clear();
        colsToCheck.Clear();
    }

    // Clears row/col
    private void ClearLine(int index, bool rowFlag)
    {
        // Clear row if flag is set
        if (rowFlag)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                if (grid[x, index] != null)
                {
                    Destroy(grid[x, index].gameObject);
                    grid[x, index] = null;
                }
            }
        }
        // Otherwise clear col
        else
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                if (grid[index, y] != null)
                {
                    Destroy(grid[index, y].gameObject);
                    grid[index, y] = null;
                }
            }
        }
    }

    // Checks if the row/col is full
    private bool IsLineFull(int index, bool rowFlag)
    {
        // Check row if flag is set
        if (rowFlag)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                if (grid[x, index] == null)
                {
                    return false;
                }
            }
        }
        // Otherwise check col
        else
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                if (grid[index, y] == null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void SpawnBlocks()
    {
        for (int i = 0; i < NUM_SPAWN; i++)
        {
            // Get a random block
            Block block = blocks[Random.Range(0, blocks.Length)];

            // Spawn the block at the respective spawn position
            Block toSpawn = Instantiate(block, pieces);
            toSpawn.Init(spawnPositions[i]);

            spawnedBlocks[i] = toSpawn;
        }

        dropCount = 0;
    }

    // Checks if the block is in a valid position
    private bool IsValidPosition(Block block)
    {
        foreach (Transform blockPiece in block.transform)
        {
            if (!IsInsideGrid(blockPiece.position) ||
                grid[(int)blockPiece.position.x, (int)blockPiece.position.y] != null)
            {
                return false;
            }
        }

        return true;
    }

    // Returns true if the position is within the grid, otherwise false
    private bool IsInsideGrid(Vector2 pos)
    {
        if (pos.x >= 0 && pos.x <= GRID_WIDTH - 1 && pos.y >= 0 && pos.y <= GRID_HEIGHT - 1)
        {
            return true;
        }

        return false;
    }

    // Returns the position with rounded coordinates
    private Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    private void CalculateSpawnPositions()
    {
        // Spawn positions are split evenly by NUM_SPAWN + 2 partitions
        // The 2 extra partitions are the edges where nothing spawns
        int partitions = NUM_SPAWN + 1;

        // X offset will be 75% of the screen
        float xOffset = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width * (float)0.75, 0)).x;
        float yOffset = (float)GRID_HEIGHT / (float)partitions;

        Debug.Log("x: " + xOffset + " y: " + yOffset);
        
        for(int i = 0; i < NUM_SPAWN; i++)
        {
            spawnPositions[i] = new Vector2(xOffset, ((float)(i+1) * yOffset) - (float)0.5);
            Debug.Log(spawnPositions[i]);
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
