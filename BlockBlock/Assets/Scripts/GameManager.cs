using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Vector2 spawnPosition;
    Vector2 spawnPosition2;
    Vector2 spawnPosition3;

    GameObject[] blocks;

    List<GameObject> blocksInGame = new List<GameObject>();

    public static GameManager gameManager = null;

    public static int gridWidth = 10;
    public static int gridHeight = 10;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    void Awake(){
      if(gameManager == null) {
        gameManager = this;
      }
      else if (gameManager != this) {
        Destroy(gameObject);
      }
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = new Vector2(-2, (float) -3.5);
        spawnPosition2 = new Vector2(4, (float) -3.5);
        spawnPosition3 = new Vector2(9, (float) -3.5);
        
        blocks = Resources.LoadAll<GameObject>("Prefabs/Blocks");

        SpawnBlock(spawnPosition);
        //SpawnBlock(spawnPosition2);
        //SpawnBlock(spawnPosition3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    ////////////////////////Spawn Blocks Functions///////////////////////////

    //Spawns Initial Blocks
    private void SpawnBlock(Vector2 pos)
    {
        // Get a random block
        GameObject block = blocks[Random.Range(0, blocks.Length)];

        // Spawn the block at the spawn position
        GameObject toSpawn = Instantiate(block, pos, Quaternion.identity);
        //toSpawn.GetComponent<Block>().SetSpawnPos(pos);

        blocksInGame.Add(block);
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
    //////////////Grid Snapping Functions/////////////////////

    //Check if the block is inside the grid
    private bool CheckIsInsideGrid(Vector2 pos)
    {
        return ((int)pos.y < gridHeight && (int)pos.y >= 0 && (int)pos.x < gridWidth && (int)pos.x >= 0);
    }

    // Checks if the current piece is inside the grid 
    // and is not conflicting with an existing block
    private bool CheckIsValidPosition()
    {
        foreach (Transform blockPiece in transform)
        {
            Vector2 pos = gameManager.Round(blockPiece.position);

            //Checks inside the grid
            if (CheckIsInsideGrid(pos) == false)
            {
                return false;
            }

            //Checks if the current place in the grid is empty or not
            if (GetTransformAtGridPos(pos) != null &&
                GetTransformAtGridPos(pos).parent != transform)
            {
                return false;
            }
        }

        return true;
    }

    public bool SnapToGrid(Block block){
        if(CheckIsValidPosition()){
          int inc = 0;
            foreach(Transform blockPiece in block.transform) {

                Vector2 newBlockPos = Round(block.transform.position);
                Vector2 blockGridPos = Round(blockPiece.position);
                Debug.Log(newBlockPos + "   " + blockGridPos + " gt " + inc);
                inc++;
                //Set the current transform piece to snap to the closest grid block
                block.transform.position = newBlockPos;
                //Set the Grid to have the transform piece
                SetGridPos(blockGridPos, blockPiece);
                //Disable each block piece
                blockPiece.GetComponent<BoxCollider2D>().enabled = false;
            }
            //Disable block
            block.enabled = false;

            return true;
        }
        return false;
    }
    //////////////Utility Functions//////////////////////////

    //Round the position of the block
    public Vector2 Round (Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }
    
    //-Getter Functions
    
    //Get the transform block at a certain location
    Transform GetTransformAtGridPos(Vector2 pos)
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

    // //Get the Initial Block Position
    // public Vector2 GetInitialBlockPos(string str)
    // {
    //   if(str == "spawnPosition"){
    //     return spawnPosition;
    //   }
    //   else if(str == "spawnPosition2"){
    //     return spawnPosition2;
    //   }
    //   else if(str == "spawnPosition3"){
    //     return spawnPosition3;
    //   }
    //   return new Vector2(12, (float) -3.5);
    // }

    // //Get the Grid 
    // public Transform[,] GetGrid()
    // {
    //     return grid;
    // }

    // //Get the grid width
    // public int GetGridWidth()
    // {
    //     return gridWidth;
    // }

    // //Get the grid height
    // public int GetGridHeight()
    // {
    //     return gridHeight;
    // }

    // public List<GameObject> GetBlocksInGame(){
    //   return blocksInGame;
    // }

    //Setters
    //Set the grid position
    public void SetGridPos(Vector2 pos, Transform blockPiece)
    {
        grid[(int)pos.x, (int)pos.y] = blockPiece;
    }
}
