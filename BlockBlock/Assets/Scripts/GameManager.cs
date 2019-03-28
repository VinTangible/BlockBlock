using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Vector2 spawnPosition;
    GameObject[] blocks;

    //public static GameManager gameManager = null;

    public static int gridWidth = 10;
    public static int gridHeight = 10;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    // Start is called before the first frame update
    void Start()
    {
        // Currently spawn position is at 75% screen width, and 50% screen height
        spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        spawnPosition.Scale(new Vector2((float).75, (float).5));
        blocks = Resources.LoadAll<GameObject>("Prefabs/Blocks");

        SpawnBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Private Functions

    private void SpawnBlock()
    {
        // Get a random block
        GameObject block = blocks[Random.Range(0, blocks.Length)];

        // Spawn the block at the spawn position
        GameObject toSpawn = Instantiate(block, spawnPosition, Quaternion.identity);
    }


    //Public Functions
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

    public Vector2 Round (Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    public bool CheckIsInsideGrid(Vector2 pos)
    {
        return ((int)pos.y < gridHeight && (int)pos.y >= 0 && (int)pos.x < gridWidth && (int)pos.x >= 0);
    }

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

    public void SetGridPos(Vector2 pos, Transform blockPiece)
    {
        grid[(int)pos.x, (int)pos.y] = blockPiece;
    }
}
