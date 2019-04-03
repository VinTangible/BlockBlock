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

    // Private Functions

    private void SpawnBlock()
    {
        // Get a random block
        GameObject block = blocks[Random.Range(0, blocks.Length)];

        // Spawn the block at the spawn position
        GameObject toSpawn = Instantiate(block, spawnPosition, Quaternion.identity);
    }

    /* If block is inside grid, returns true */
    private bool IsInsideGrid(Vector2 pos){
        //checks if block is within the grid
        return ((int)pos.x >= 0 && (int)pos.x < GRID_WIDTH && (int)pos.y >= 0 && (int)pos.y < GRID_HEIGHT);
    }

   private bool IsValidPosition(GameObject block){
        //iterates through each transform(individual block pieces) and check if it is in a valid position 
        //(e.g. inside grid and not overlapping another piece)
        foreach(Transform blockPiece in block.transform){
            Vector2 pos = Round(blockPiece.position);

            //checks if inside grid and grid position is not occupied
            if(IsInsideGrid(pos) && grid[(int)pos.x,(int)pos.y] == null){
                return true;
            }
        }
        return false;
    }

    /* Rounds block x/y position and returns Vector2 */
    public Vector2 Round(Vector2 pos){
        //Rounds block piece position
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    /* snaps block to grid */
    private void SnapToGrid(){

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
