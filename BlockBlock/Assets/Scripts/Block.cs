﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool allowRotation;
    public bool limitRotation;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(SnapToGrid())
            {
                FindObjectOfType<GameManager>().SpawnBlock();
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
        return false;
    }
}
