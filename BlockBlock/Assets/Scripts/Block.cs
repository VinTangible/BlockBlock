using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector2 offset;
    public bool allowRotation = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
        /*if(Input.GetMouseButton(0)){
            Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = newPosition;
        }*/

    }

    void OnMouseDown(){
        // Calculate offset of the mouse position and starting position of parent when drag begins
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    void OnMouseDrag(){
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = newPosition - offset;
    }

    void OnMouseUp(){
        GameManager.manager.SnapToGrid(this);
        GameManager.manager.Clear();
    }
}