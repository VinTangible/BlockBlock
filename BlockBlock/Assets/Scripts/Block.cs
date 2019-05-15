using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool allowRotation;
    public bool limitRotation;

    Vector2 spawnPosition;
    GameManager gameManager = GameManager.gameManager;

    void Awake() 
    {
        spawnPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SetSortingLayer("Clicked", 2);
            gameManager.SnapToGrid(this);
        }
    }

    //Rotate block (Different combinations)
    public void RotateBlock()
    {
        int[] allowDegree = { 0, 90, 180, 270 };
        int[] limitDegree = {0, 90};
        int randomIndex = 0;

        if(allowRotation && limitRotation)
        {
            //Debug.Log("LIMIT Here");
            randomIndex = Random.Range(0, limitDegree.Length);
            GetComponent<Transform>().Rotate(0, 0, limitDegree[randomIndex]);
        }
        else if(allowRotation)
        {
            //Debug.Log("Allow Here");
            randomIndex = Random.Range(0, allowDegree.Length);
            GetComponent<Transform>().Rotate(0, 0, allowDegree[randomIndex]);
        }
    }

    public void SetSortingLayer(string sortingLayerName, int sortingOrder) {
        foreach(Transform blockPiece in transform) {
            blockPiece.GetComponentInParent<SpriteRenderer>().sortingLayerName = sortingLayerName;
            blockPiece.GetComponentInParent<SpriteRenderer>().sortingOrder = sortingOrder;
        }
        //transform.GetComponentInParent<SpriteRenderer>().sortingLayerName = sortingLayerName;
        //transform.GetComponentInParent<SpriteRenderer>().sortingOrder = sortingOrder;
    }
    /////////////////Get Functions////////////////////
    public Vector2 GetBlockSpawnPosition(){
        return spawnPosition;
    }

}
