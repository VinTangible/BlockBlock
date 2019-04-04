using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPiece : MonoBehaviour
{
    private Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    //CODE MOVED TO BLOCK.CS

   void OnMouseDown()
    {
        // Calculate offset of the mouse position and starting position of parent when drag begins
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position;
        Debug.Log("BlockPiece.cs OnMouseDown Registered");
    }

    void OnMouseDrag()
    {
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.parent.position = newPosition - offset;
    } 
    */

    void OnDestroy()
    {
        // Destroy parent block if this is the last piece left to destroy
        if (transform.parent != null && transform.parent.childCount <= 1) {
            Destroy(transform.parent.gameObject);
        }
    }    
}
