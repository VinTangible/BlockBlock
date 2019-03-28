﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool allowRotation;
    public bool limitRotation;

    Vector2 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = FindObjectOfType<GameManager>().GetInitialBlockPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(SnapToGrid())
            {
                SpawnNextBlock(spawnPosition);
            }
        }
    }

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
