using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool allowRotation;
    public bool limitRotation;
    Transform[,] grid;
    int gridHeight = 0;
    int gridWidth = 0;

    List<int> fullRowArr = new List<int>();
    List<int> fullColArr = new List<int>();

    Vector2 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = FindObjectOfType<GameManager>().GetInitialBlockPos();
        grid = FindObjectOfType<GameManager>().GetGrid();
        gridHeight = FindObjectOfType<GameManager>().GetGridHeight();
        gridWidth = FindObjectOfType<GameManager>().GetGridWidth();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(SnapToGrid())
            {
                clearRowCol();
                SpawnNextBlock(spawnPosition);
            }
        }
    }

    //Grid Snapping
    bool CheckIsValidPosition()
    {
        foreach (Transform blockPiece in transform)
        {
            Vector2 pos = FindObjectOfType<GameManager>().Round(blockPiece.position);

            if (FindObjectOfType<GameManager>().CheckIsInsideGrid(pos) == false)
            {
                return false;
            }

            if (FindObjectOfType<GameManager>().GetTransformAtGridPos(pos) != null &&
                FindObjectOfType<GameManager>().GetTransformAtGridPos(pos).parent != transform)
            {
                return false;
            }
        }

        return true;
    }

    bool SnapToGrid()
    {
        if (CheckIsValidPosition())
        {
            foreach(Transform blockPiece in transform)
            {
                Vector2 newBlockPos = FindObjectOfType<GameManager>().Round(transform.position);
                Vector2 blockGridPos = FindObjectOfType<GameManager>().Round(blockPiece.position);

                transform.position = newBlockPos;

                FindObjectOfType<GameManager>().SetGridPos(blockGridPos, blockPiece);
                
                blockPiece.GetComponent<BoxCollider2D>().enabled = false;    

            }

            enabled = false;

            return true;
        }

        //revert to spawn position once it is not valid position
        transform.position = spawnPosition;
        return false;
    }

    //Clearing Rows and Columns

    //Check for both full row and col
    bool isFullRowColAt(int x, int y)
    {
        //Transform[,] grid = FindObjectOfType<GameManager>().GetGrid();
        //int gridHeight = FindObjectOfType<GameManager>().GetGridHeight();
        //int gridWidth = FindObjectOfType<GameManager>().GetGridWidth();

        //Check for both row and col
        //If either of it is null, return false;
        for(int row = 0; row <  gridWidth; row++)
        {
            if(grid[row, y] == null)
            {
                return false;
            }
      
            for(int col = 0; col < gridHeight; col++)
            {
                if(grid[x, col] == null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    //Checking full row
    bool isFullRowAt(int y)
    {
        //Transform[,] grid = FindObjectOfType<GameManager>().GetGrid();
        //int gridHeight = FindObjectOfType<GameManager>().GetGridHeight();
        //int gridWidth = FindObjectOfType<GameManager>().GetGridWidth();

        for(int row = 0; row < gridWidth; row++)
        {
            if(grid[row , y] == null)
            {
                return false;
            }
        }
        return true;
    }

    //Checking full col
    bool isFullColAt(int x)
    {

        for(int col = 0; col < gridHeight; col++)
        {
            if(grid[x, col] == null)
            {
                return false;
            }
        }
        return true;
    }

    //Clearing rows/col
    /*void ClearRowsCol()
    {
        for(int row = 0; row < gridWidth; row++)
        {
            for(int col = 0; col < gridHeight; col++)
            {
                if(isFullRowColAt(row, col))
                {
                    Destroy(grid[row, col].gameObject);
                    grid[row, col] = null;
                }
                else if (isFullRowAt(col))
                {
                    Destroy(grid[])
                }
            }
        }
    }*/

    void addingFullRows()
    {
        for (int row = 0; row < gridWidth; row++)
        {
            for (int col = 0; col < gridHeight; col++)
            {
                if (isFullRowAt(col))
                {
                    fullRowArr.Add(col);
                }
                else if (isFullColAt(row))
                {
                    fullColArr.Add(row);
                }
            }
        }
    }

    void clearRowCol()
    {
        foreach(int el in fullRowArr)
        {
            for(int row = 0; row < gridHeight; row++)
            {
                Destroy(grid[row, el].gameObject);
                grid[row, el] = null;
            }
            fullRowArr.Remove(el);
        }

        foreach (int el in fullColArr)
        {
            for (int col = 0; col < gridHeight; col++)
            {
                Destroy(grid[el, col].gameObject);
                grid[el, col] = null;
            }
            fullRowArr.Remove(el);
        }
    }

    //Block Spawn
    void SpawnNextBlock(Vector2 pos)
    {
        //Debug.Log("I AM HERE");
        GameObject[] blocks = Resources.LoadAll<GameObject>("Prefabs/Blocks");
        GameObject block = blocks[Random.Range(0, blocks.Length)];
        GameObject nextBlock = (GameObject)Instantiate(block, pos, Quaternion.identity);

        RotateBlock(nextBlock.GetComponentInParent<Transform>());

    }

    void RotateBlock(Transform block)
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


}
