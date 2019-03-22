using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 10;

    public static GameManager gameManager = null;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
        }
        else if(gameManager != this)
        {
            Destroy(gameObject);
        }
    }

    public void SpawnNextBlock(Vector2 startingPosition)
    {
        GameObject nextBlock = (GameObject)Instantiate(
                    Resources.Load(GetRandomBlock(), typeof(GameObject)),
                        startingPosition, Quaternion.identity);
    }

    public bool SnapToGrid(Block block)
    {
        Debug.Log("Snapping to grid...");

        if(IsInsideGrid(block))
        {
            foreach(Transform blockPiece in block.transform)
            {
                Vector2 roundedPosition = Round(block.transform.position);

                block.transform.position = roundedPosition;

                blockPiece.GetComponent<BoxCollider2D>().enabled = false;
            }

            return true;
        }

        // else, bring block back to original spawn position
        block.transform.position = block.startingPosition;
        return false;
    }

    // checks if a block is inside the grid by iterating through
    // all of its individual pieces and checking their positions
    public bool IsInsideGrid(Block block)
    {
        int blockNum = 1;
        foreach(Transform blockPiece in block.transform)
        {
            Vector2 roundedPosition = Round(blockPiece.transform.position);

            Debug.Log("Checking block " + blockNum);
            Debug.Log("X: " + blockPiece.position.x + ", Y: " + blockPiece.position.y);

            if((int) roundedPosition.x < 0 ||
               (int) roundedPosition.x > gridWidth - 1 ||
               (int) roundedPosition.y < 0 || 
               (int) roundedPosition.y > gridHeight - 1)
            {
                return false;
            }

            ++blockNum;
        }

        return true;
    }

    // Rounds the x and y positions to the nearest integer
    public Vector2 Round(Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }

    string GetRandomBlock()
    {
        string prefabPath = "Prefabs\\Blocks\\";

        string[] blocks = {"El2", "El3", "Long2", "Long3", "Long4", "Long5", 
            "Square1", "Square2", "Square3"};

        int randomBlock = Random.Range(0, 9);

        return prefabPath + blocks[randomBlock];
    }

    // Chooses a random starting position for each block.
    // All of the starting positions have the same y value,
    // so we're choosing pre-determined x values.
    Vector2 GetRandomStartPosition()
    {
        float[] xPositions = {-3.5f, 1.5f, 5f, 11f};

        int randomXPosition = Random.Range(0, 4);

        return new Vector2(xPositions[randomXPosition], -2.5f);
    }
}
