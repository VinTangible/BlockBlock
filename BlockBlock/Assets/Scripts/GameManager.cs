using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Vector2 spawnPosition;
    GameObject[] blocks;


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
}
