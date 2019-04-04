using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Vector2 spawnPosition;
    //holds all types of blocks that can be spawned
    Block[] blocks;

    public static int GRID_WIDTH = 10;
    public static int GRID_HEIGHT = 10;

    public bool[] rowsToDelete = new bool[GRID_HEIGHT];
    public bool[] colToDelete = new bool[GRID_WIDTH];

    public static string prefabPath = "Prefabs/Blocks";

    //holds occupied/null grid positions
    public static Transform[,] grid = new Transform[GRID_WIDTH,GRID_HEIGHT];

    public static GameManager manager = null;

    // Start is called before the first frame update
    void Start()
    {
        // Currently spawn position is at 75% screen width, and 50% screen height
        spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height)); //why do we need Camera.main.ScreenToWorldPoint?
        spawnPosition.Scale(new Vector2((float).75, (float).5));

        //holds all different block types to be spawned
        blocks = Resources.LoadAll<Block>(prefabPath);

        SpawnBlock();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake(){
        //create static singleton instance of game manager to be called in other scripts
        if(manager == null){
            manager = this;
        }
        //if manager is not equal to current instance of gameManager, then destroy it
        else if(manager != this){
            Destroy(gameObject);
        }
    }

    /*
    Description: Spawns new block
    Where Called: Start(), SnapToGrid()
     */
    private void SpawnBlock()
    {
        // Get a random block
        Block block = blocks[Random.Range(0, blocks.Length)];

        // Spawn the block at the spawn position
        Block toSpawn = Instantiate(block, spawnPosition, Quaternion.identity);

/* How do I rotate using a function? Function would only rotate the shallow copy of the block. How to rotate the original copy?*/
        //checks if block can rotate
        if(toSpawn.allowRotation){
            //rotate spawned block
            toSpawn.transform.Rotate(0,0,getRotationVal());
        }
    }

    /*
    Description: Gets value to rotate block
    Where Called: SpawnBlock()
     */
    private int getRotationVal(){
        //declare array to hold possible rotations
        int[] rotationValues = {0,90,180,270};

        return rotationValues[Random.Range(0,rotationValues.Length)];
    }

    /* 
    Description: If entire block (all block pieces) is inside grid, returns true.
    Where Called: IsValidPosition()
    */
    private bool IsInsideGrid(Block block){
        //checks if block is within the grid
        foreach(Transform blockPiece in block.transform){
            Vector2 pos = Round(blockPiece.position);

            if(pos.x < 0 || pos.x >= GRID_WIDTH || pos.y < 0 || pos.y >= GRID_HEIGHT){
                //Debug.Log("IsInsideGrid returned FALSE");
                return false;
            }
        }
        //Debug.Log("IsInsideGrid returned TRUE");
        return true;
    }

    /*
    Description: Checks if block position is valid (e.g. grid spot is empty)
    Where Called: SnapToGrid()
    */
   private bool IsValidPosition(Block block){
       //iterates through each transform(individual block pieces) and check if it is in a valid position 
       foreach(Transform blockPiece in block.transform){
           Vector2 pos = Round(blockPiece.position);
           //checks if grid position is not occupied
           if(grid[(int)pos.x,(int)pos.y] != null){
               //Debug.Log("IsValidPosition returned FALSE");
               return false;
               }
        }
        //Debug.Log("IsValidPosition returned TRUE");
        return true;
    }

    /* 
    Description: Rounds block x/y position and returns Vector2 
    Where Called: IsInsideGrid(), IsValidPosition(), SnapToGrid(), UpdateGrid()
    */
    public Vector2 Round(Vector2 pos){
        //Rounds block piece position
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    /* 
    Description: Snaps block to grid
    Where Called: Block.cs -> OnMouseUp()
    */
    public void SnapToGrid(Block block){
        //checks if block is inside grid. If it isn't, then reset to spawn location
        if(IsInsideGrid(block) && IsValidPosition(block)){
            block.transform.position = Round(block.transform.position);

            //disables dropped block (prevents movement)
            block.GetComponent<BoxCollider2D>().enabled = false;
            
            UpdateGrid(block);
            SpawnBlock();
        }
        else{
            block.transform.position = spawnPosition;
        }
    }

    /*
    Description: updates grid to keep track of which spots are filled.
    Where Called: SnapToGrid()
     */
    public void UpdateGrid(Block block){
        //finds position for each block piece within block. Updates grid to hold block piece. 
        foreach(Transform blockPiece in block.transform){
            Vector2 pos = Round(blockPiece.position);

            grid[(int)pos.x,(int)pos.y] = blockPiece;
            //Debug.Log("grid[" + blockPiece.position.x + "," + blockPiece.position.y + "] updated.");
        }
    }

    /*
    Description: Checks if row is full
    Where Called: Clear()
    */
    private static bool CheckRow(int y){
        for(int x = 0; x < GRID_WIDTH; x++){
            if(grid[x,y] == null){
                return false;
            }
        }
        return true;
    }

    /*
    Description: Checks if column is full
    Where Called: Clear()
    */
    private static bool CheckColumn(int x){
        for(int y = 0; y < GRID_HEIGHT; y++){
            if(grid[x,y] == null){
                return false;
            }
        }
        return true;
    }

    /*
    Description: Deletes row of blocks
    Where Called: Clear()
    */
    private static void DeleteRow(int y){
        for(int x = 0; x < GRID_WIDTH; x++){
            //this check fixes null exception error. This check is used when both column and row
            //are cleared at the same time. Since DeleteColumn() is called first, the block piece
            //in the grid is destroyed. Check prevents function from trying to destroy null object.
            if(grid[x,y] == null){
                continue;
            }
            Destroy(grid[x,y].gameObject);

            grid[x,y] = null;
        }
    }
    /*
    Description: Deletes column of blocks
    Where Called: Clear()
    */
    private static void DeleteColumn(int x){
        for(int y = 0; y < GRID_HEIGHT; y++){
            Destroy(grid[x,y].gameObject);

            grid[x,y] = null;
        }
    }

    /*
    Description: Iterates through grid and clears full row/column
    Where Called: Block.cs -> OnMouseDown()
    */
    public void Clear(){
        //goes through grid and marks which row/col should be deleted
        for(int x = 0; x < GRID_WIDTH; x++){
            if(CheckColumn(x)){
                colToDelete[x] = true;
            }
        }
        for(int y = 0; y < GRID_HEIGHT; y++){
            if(CheckRow(y)){
                rowsToDelete[y] = true;
            }
        }

        //deletes row/col. Because DeleteRow is called second, there is a null exception error when deleting
        //both column and row at the same time. Resolved by adding check in DeleteRow(). If for loops
        //were switched, then function should be added to DeleteColumn() instead
        for(int x = 0; x < GRID_WIDTH; x++){
            if(colToDelete[x]){
                DeleteColumn(x);
                colToDelete[x] = false;
            }
        }
        for(int y = 0; y < GRID_HEIGHT; y++){
            if(rowsToDelete[y]){
                DeleteRow(y);
                rowsToDelete[y] = false;
            }
        }
    }
}
