using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool allowRotation;
    public bool limitRotation;

    Vector2 spawnPosition;
    GameManager gameManager = GameManager.gameManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(gameManager.SnapToGrid(this))
            {
                gameManager.AddingFullRowsInColRow();
                gameManager.ClearRowCol();
                gameManager.SpawnBlock(spawnPosition);
            }
        }
    }

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
        }
        else if(allowRotation)
        {
            randomIndex = Random.Range(0, allowDegree.Length);
            block.Rotate(0, 0, allowDegree[randomIndex]);
        }
    }
}
