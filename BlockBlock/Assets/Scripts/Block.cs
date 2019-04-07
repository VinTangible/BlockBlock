using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool allowRotation;
    public bool limitRotation;
    // Transform[,] grid;
    // int gridHeight = 0;
    // int gridWidth = 0;

    // List<int> fullRowArr = new List<int>();
    // List<int> fullColArr = new List<int>();

    // List<GameObject> blocksInGame;

    Vector2 spawnPosition;
    // Vector2 spawnPosition2;
    // Vector2 spawnPosition3;

    GameManager gameManager = GameManager.gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //spawnPosition = gameManager.GetInitialBlockPos("spawnPosition");
        //spawnPosition2 = gameManager.GetInitialBlockPos("spawnPosition2");
        //spawnPosition3 = gameManager.GetInitialBlockPos("spawnPosition3");
        // grid = gameManager.GetGrid();
        // gridHeight = gameManager.GetGridHeight();
        // gridWidth = gameManager.GetGridWidth();
        // blocksInGame = gameManager.GetBlocksInGame();
        spawnPosition = gameManager.GetSpawnPosition(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log(gameManager.SnapToGrid(this));
            if(gameManager.SnapToGrid(this))
            {
                gameManager.AddingFullRowsInColRow();
                gameManager.ClearRowCol();
                gameManager.SpawnBlock(spawnPosition);
            }
        }
    }

    //Grid Snapping
    //Checks if the current piece is inside the grid 
    //and is not conflicting with an existing block
    // bool CheckIsValidPosition()
    // {
    //     foreach (Transform blockPiece in transform)
    //     {
    //         Vector2 pos = gameManager.Round(blockPiece.position);

    //         //Checks inside the grid
    //         if (gameManager.CheckIsInsideGrid(pos) == false)
    //         {
    //             return false;
    //         }

    //         //Checks if the current place in the grid is empty or not
    //         if (gameManager.GetTransformAtGridPos(pos) != null &&
    //             gameManager.GetTransformAtGridPos(pos).parent != transform)
    //         {
    //             return false;
    //         }
    //     }

    //     return true;
    // }

    //Snaps to the grid if it is a valid position
    //Returns true if it snaps
    //Returns false if it doesn't snaps
    // bool SnapToGrid()
    // {
    //     if (CheckIsValidPosition())
    //     {
    //         foreach(Transform blockPiece in transform)
    //         {
    //             Vector2 newBlockPos = gameManager.Round(transform.position);
    //             Vector2 blockGridPos = gameManager.Round(blockPiece.position);

    //             transform.position = newBlockPos;

    //             gameManager.SetGridPos(blockGridPos, blockPiece);
                
    //             blockPiece.GetComponent<BoxCollider2D>().enabled = false;    

    //         }

    //         enabled = false;

    //         return true;
    //     }

    //     //revert to spawn position once it is not valid position
    //     //transform.position = spawnPosition;
    //     SpawnBackToInitialPos();
    //     return false;
    // }

    // void SpawnBackToInitialPos() {
    //   GetComponentInParent<Transform>().position = spawnPosition;
    // }

    //Clearing Rows and Columns

    //Checking full row
    //Returns true if the row is full
    // bool isFullRowAt(int y)
    // {
    //     for(int row = 0; row < gridWidth; row++)
    //     {
    //         if(grid[row , y] == null)
    //         {
    //             return false;
    //         }
    //     }
    //     return true;
    // }

    //Checking full col
    //Return true if the current column is full
    // bool isFullColAt(int x)
    // {
    //     for(int col = 0; col < gridHeight; col++)
    //     {
    //         if(grid[x, col] == null)
    //         {
    //             return false;
    //         }
    //     }
    //     return true;
    // }

    //Adds the row number and col number into a list if that
    //row or column if full
    // void AddingFullRows()
    // {
    //     //Adding row number into list
    //     for(int row = 0; row < gridWidth; row++)
    //     {
    //         if (isFullColAt(row))
    //         {
    //             fullColArr.Add(row);
    //         }
    //     }

    //     //Adding col number into list
    //     for(int col = 0; col < gridHeight; col++)
    //     {
    //         if (isFullRowAt(col))
    //         {
    //             fullRowArr.Add(col);
    //         }
    //     }
    // }

    //Go through each row and column number in the list
    //and destroying and deleting the blocks from the grid 
    // void ClearRowCol()
    // {
    //     //Can't manipulate data structure at the same time as transversing
    //     //Gives Error

    //     foreach(int el in fullRowArr)
    //     {
    //         //Debug.Log(el + " " + fullRowArr);
    //         for(int row = 0; row < gridWidth; row++)
    //         {
    //           //making sure that we arent destroying a null piece
    //           if(grid[row, el] != null){
    //             Destroy(grid[row, el].gameObject);
    //             grid[row, el] = null;
    //           }
    //         }
    //     }

    //     foreach (int el in fullColArr)
    //     {
    //         Debug.Log(el + " " + fullColArr.ToArray());
    //         for (int col = 0; col < gridHeight; col++)
    //         {
    //           //making sure that we arent destroying a null piece
    //           if(grid[el, col] != null){
    //             Destroy(grid[el, col].gameObject);
    //             grid[el, col] = null;
    //           }
    //         }
    //     }

    // }

    //Block Spawn
    //Spawns the next block and rotates it (different combinations of blocks)
    // void SpawnNextBlock(Vector2 pos)
    // {
    //     //Debug.Log("I AM HERE");
    //     GameObject[] blocks = Resources.LoadAll<GameObject>("Prefabs/Blocks");
    //     GameObject block = blocks[Random.Range(0, blocks.Length)];
    //     GameObject nextBlock = (GameObject)Instantiate(block, pos, Quaternion.identity);
    //     nextBlock.GetComponent<Block>().SetSpawnPos(pos);
    //     nextBlock.GetComponent<Block>().SpawnBackToInitialPos();
    //     RotateBlock(nextBlock.GetComponentInParent<Transform>());

    // }

    //Rotate block (Different combinations)
    public void RotateBlock(Transform block)
    {
        int[] allowDegree = { 0, 90, 180, 270 };
        int[] limitDegree = {0, 90};
        int randomIndex = 0;

        if(allowRotation && limitRotation)
        {

            randomIndex = Random.Range(0, limitDegree.Length);
            block.Rotate(0, 0, limitDegree[randomIndex]);
            //Debug.Log("limit " + limitDegree[randomIndex]);
        }
        else if(allowRotation)
        {
            randomIndex = Random.Range(0, allowDegree.Length);
            block.Rotate(0, 0, allowDegree[randomIndex]);
            //Debug.Log("allow " + allowDegree[randomIndex]);
        }
    }

    //Setters
    // public void SetSpawnPos(Vector2 pos) {
    //   spawnPosition = pos;
    // }

}
