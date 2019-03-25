using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton GameManager instance
    public static GameManager instance = null;

    public static int gridWidth = 10;
    public static int gridHeight = 10;

    // Name of sorting layers
    public static string upLayer = "Up";
    public static string downLayer = "Down";

    Vector2 spawnPosition;

    GameObject[] blocks;    // Used to store Block prefabs
    Transform pieces;       // Used to store Blocks in Scene Hierarchy

    Transform[,] grid = new Transform[gridWidth, gridHeight];
    HashSet<int> rowsToCheck = new HashSet<int>();
    HashSet<int> colsToCheck = new HashSet<int>();

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
        // Currently spawn position is at 75% screen width, and 50% screen height
        spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        spawnPosition.Scale(new Vector2((float).75, (float).5));
        blocks = Resources.LoadAll<GameObject>("Prefabs/Blocks");

        // Create transform to hold all block pieces
        pieces = new GameObject("Pieces").transform;

        // Spawn inital block
        SpawnBlock();
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

            // Add block pieces to the grid
            foreach (Transform blockPiece in block.transform)
            {
                grid[(int)blockPiece.position.x, (int)blockPiece.position.y] = blockPiece;

                // A block was placed in this row/col so mark as needing to check
                rowsToCheck.Add((int)blockPiece.position.y);
                colsToCheck.Add((int)blockPiece.position.x);
            }

            ClearLines();

            // Spawn a new one (TODO only spawn once all blocks are placed)
            SpawnBlock();
        }
        else
        {
            block.transform.position = GetSpawnPosition(block.gameObject);
        }

    }

    // Private Functions

    // Clears all the full rows/cols
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
            for (int x = 0; x < gridWidth; x++)
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
            for (int y = 0; y < gridHeight; y++)
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
            for (int x = 0; x < gridWidth; x++)
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
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[index, y] == null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    // Get adjusted spawn position used to center the block at spawn
    private Vector2 GetSpawnPosition(GameObject block)
    {
        BoxCollider2D box = block.GetComponent<BoxCollider2D>();

        // Apply (.5, .5) offset because (.5, .5) of the block sticks out
        return spawnPosition - box.size/2 + new Vector2(.5f, .5f);
    }

    private void SpawnBlock()
    {
        // Get a random block
        GameObject block = blocks[Random.Range(0, blocks.Length)];

        // Spawn the block at the spawn position
        GameObject toSpawn = Instantiate(block, GetSpawnPosition(block), Quaternion.identity);
        toSpawn.transform.SetParent(pieces);
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
        if (pos.x >= 0 && pos.x <= gridWidth - 1 && pos.y >= 0 && pos.y <= gridHeight - 1)
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
}
