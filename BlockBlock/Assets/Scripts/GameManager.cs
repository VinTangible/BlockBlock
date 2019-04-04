using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Vector2 spawnPosition;
    Vector2 spawnPosition2;
    Vector2 spawnPosition3;

    GameObject[] blocks;

    //public static GameManager gameManager = null;

    public static int gridWidth = 10;
    public static int gridHeight = 10;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    // Start is called before the first frame update
    void Start()
    {
        // Currently spawn position is at 75% screen width, and 50% screen height
        //spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        //spawnPosition.Scale(new Vector2((float).75, (float).5));
        spawnPosition = new Vector2(-2, (float) -3.5);
        spawnPosition2 = new Vector2(4, (float) -3.5);
        spawnPosition3 = new Vector2(9, (float) -3.5);
        
        blocks = Resources.LoadAll<GameObject>("Prefabs/Blocks");

        SpawnBlock(spawnPosition);
        SpawnBlock(spawnPosition2);
        SpawnBlock(spawnPosition3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Private Functions

    //Spawns Initial Blocks
    private void SpawnBlock(Vector2 pos)
    {
        // Get a random block
        GameObject block = blocks[Random.Range(0, blocks.Length)];

        // Spawn the block at the spawn position
        GameObject toSpawn = Instantiate(block, pos, Quaternion.identity);
    }


    //Public Functions

    //Updates the grid on which positions in the grid are filled
    public void UpdateGrid(BlockPiece block)
    {
        foreach(Transform blockPiece in block.transform)
        {
            Vector2 pos = Round(blockPiece.position);

            if(CheckIsInsideGrid(pos))
            {
                grid[(int)pos.x, (int)pos.y] = blockPiece;
            }
        }
    }

    //Round the position of the block
    public Vector2 Round (Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    //Check if the block is inside the grid
    public bool CheckIsInsideGrid(Vector2 pos)
    {
        return ((int)pos.y < gridHeight && (int)pos.y >= 0 && (int)pos.x < gridWidth && (int)pos.x >= 0);
    }

    //Getters
    //Get the transform block at a certain location
    public Transform GetTransformAtGridPos(Vector2 pos)
    {
        if (!CheckIsInsideGrid(pos))
        {
            return null;
        }
        else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }

    //Get the Initial Block Position
    public Vector2 GetInitialBlockPos()
    {
        return spawnPosition;
    }

    //Get the Grid 
    public Transform[,] GetGrid()
    {
        return grid;
    }

    //Get the grid width
    public int GetGridWidth()
    {
        return gridWidth;
    }

    //Get the grid height
    public int GetGridHeight()
    {
        return gridHeight;
    }

    //Setters
    //Set the grid position
    public void SetGridPos(Vector2 pos, Transform blockPiece)
    {
        grid[(int)pos.x, (int)pos.y] = blockPiece;
    }

}
