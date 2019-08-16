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

    public GameObject playAgainButton;

    int dropCount = 0;

    Block[] blocks;         // Used to store Block prefabs
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
        playAgainButton.SetActive(false);
        CalculateSpawnPositions();

        // Load block prefabs
        blocks = Resources.LoadAll<Block>("Prefabs/Blocks/");

        // Create transform to hold all block pieces
        pieces = new GameObject("Pieces").transform;

        // Spawn inital block
        SpawnBlocks();
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Drops the Block onto the board if valid
    public void DropPiece(Block block)
    {
        // Round the block's position to align with grid
        RoundPosition(block.transform);

        if (IsValidPosition(block))
        {
            // Disable the dropped block
            block.GetComponent<BoxCollider2D>().enabled = false;

            // Increase drop counter
            dropCount++;

            // Add block pieces to the grid
            foreach (Transform blockPiece in block.transform)
            {
                int roundedX = Mathf.RoundToInt(blockPiece.position.x);
                int roundedY = Mathf.RoundToInt(blockPiece.position.y);

                // The Y represents the rows and the X represents the cols
                grid[roundedX, roundedY] = blockPiece;

                // A block was placed in this row/col so mark as needing to check
                // The X is the col, and the Y is the row
                rowsToCheck.Add(roundedY);
                colsToCheck.Add(roundedX);
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
            if (block != null && block.GetComponent<BoxCollider2D>().enabled)
            {
                // Scale block to Up scale before checking
                block.ResetScale();

                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    for (int y = 0; y < GRID_HEIGHT; y++)
                    {
                        // Move block to position and check if it is a valid position
                        block.transform.position = new Vector2(x, y);
                        RoundPosition(block.transform);

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
                    GameObject blockPiece = grid[x, index].gameObject;
                    Animator blockPieceAnim = blockPiece.GetComponent<Animator>();

                    // set trigger to transition to the destroy animation
                    blockPieceAnim.SetTrigger("DestroyAnimation");

                    // wait to allow the animation to finish before destroying the block piece
                    Destroy(blockPiece, blockPieceAnim.GetCurrentAnimatorClipInfo(0).Length);
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
                    GameObject blockPiece = grid[index, y].gameObject;
                    Animator blockPieceAnim = blockPiece.GetComponent<Animator>();

                    // set trigger to transition to the destroy animation
                    blockPieceAnim.SetTrigger("DestroyAnimation");

                    // wait to allow the animation to finish before destroying the block piece
                    Destroy(blockPiece, blockPieceAnim.GetCurrentAnimatorClipInfo(0).Length);
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
            Block toSpawn = Instantiate(block, spawnPositions[i], Quaternion.identity);
            spawnedBlocks[i] = toSpawn;
            toSpawn.transform.SetParent(pieces);

            // Apply random rotation to block
            RotateBlock(toSpawn);

            // iterate through each of the block's child pieces to trigger their spawn animations
            foreach(Transform blockPiece in toSpawn.transform)
            {
                // set the trigger to transition to the spawn animation
                blockPiece.gameObject.GetComponent<Animator>().SetTrigger("SpawnAnimation");
            }
        }

        dropCount = 0;
    }

    // Checks if the block is in a valid position
    private bool IsValidPosition(Block block)
    {
        foreach (Transform blockPiece in block.transform)
        {
            if (!IsInsideGrid(blockPiece.position) ||
                grid[Mathf.RoundToInt(blockPiece.position.x), Mathf.RoundToInt(blockPiece.position.y)] != null)
            {
                return false;
            }
        }

        return true;
    }

    // Returns true if the position is within the grid, otherwise false
    private bool IsInsideGrid(Vector2 pos)
    {
        int roundedX = Mathf.RoundToInt(pos.x);
        int roundedY = Mathf.RoundToInt(pos.y);

        if (roundedX >= 0 && roundedX <= GRID_WIDTH - 1 &&
            roundedY >= 0 && roundedY <= GRID_HEIGHT - 1)
        {
            return true;
        }

        return false;
    }

    // Rotates block randomly by 90 degree increments around z axis
    private void RotateBlock(Block block)
    {
        int[] rotations = new int[] {0, 90, 180, 270};
        block.transform.Rotate(0,0, rotations[Random.Range(0, block.numRotations + 1)]);
    }

    // Rounds the parent and all its children to the nearest position (whole number)
    private void RoundPosition(Transform parent)
    {
        if (parent.childCount > 0)
        {
            Transform child = parent.GetChild(0);

            Vector2 roundedPos = new Vector2(Mathf.Round(child.position.x), Mathf.Round(child.position.y));
            Vector2 offset = (Vector2)child.transform.position - roundedPos;
            parent.position = (Vector2)parent.position - offset;
        }
        else
        {
            parent.position = new Vector2(Mathf.Round(parent.position.x), Mathf.Round(parent.position.y));
        }
    }

    private void CalculateSpawnPositions()
    {
        // Spawn positions are split evenly by NUM_SPAWN + 2 partitions
        // The 2 extra partitions are the edges where nothing spawns
        int partitions = NUM_SPAWN + 2;
        float leftOffset = (GRID_WIDTH - 1) / partitions;
        float widthAfterOffset = (GRID_WIDTH - 1) - (2 * leftOffset);

        float yOffset = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height * (float)0.15)).y;
        
        for(int i = 0; i < NUM_SPAWN; i++)
        {
            float xOffset = (float)i/(float)(NUM_SPAWN-1) * widthAfterOffset + leftOffset;
            spawnPositions[i] = new Vector2(xOffset, yOffset);
        }
    }

    private void GameOver()
    {
        DisableBlocks();
        playAgainButton.SetActive(true);
    }
    
    private void DisableBlocks()
    {
        foreach (Block block in spawnedBlocks)
        {
            block.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
