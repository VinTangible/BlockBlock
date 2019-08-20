using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector2 offset;    
    public Vector2 spawnPosition;
    public bool allowRotation = true;

    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update(){

    }

    void OnMouseDown(){
        // Calculate offset of the mouse position and starting position of parent when drag begins
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        IncrementSortingOrder();
    }

    void OnMouseDrag(){
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = newPosition - offset;
    }

    void OnMouseUp(){
        //snaps block to grid
        GameManager.manager.SnapToGrid(this);
        //clears any full rows
        GameManager.manager.Clear();

        DecrementSortingOrder();
    }

    void IncrementSortingOrder(){
        //increments sorting order for each sprite within Block so it overlaps all other blocks
        foreach(Transform form in transform){
            form.GetComponent<SpriteRenderer>().sortingOrder++;
        }
    }

    void DecrementSortingOrder(){
        //decrements sorting order for each sprite within Block so it can be overlapped
        foreach(Transform form in transform){
            form.GetComponent<SpriteRenderer>().sortingOrder--;
        }
    }
}