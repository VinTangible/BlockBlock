using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Vector2 spawnPosition;
    //holds all types of blocks that can be spawned
    GameObject[] blocks;

    public static int GRID_WIDTH = 10;
    public static int GRID_HEIGHT = 10;

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
        blocks = Resources.LoadAll<GameObject>("Prefabs/Blocks");

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
        GameObject block = blocks[Random.Range(0, blocks.Length)];

        // Spawn the block at the spawn position
        GameObject toSpawn = Instantiate(block, spawnPosition, Quaternion.identity);
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

    private void CheckRows(){

    }

    private void CheckColumns(){

    }

    private void DeleteRow(){

    }

    private void DeleteColumn(){

    }

    private void Clear(){

    }
}
