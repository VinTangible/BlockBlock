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

    void OnDestroy()
    {
        // Destroy parent block if this is the last piece left to destroy
        if (transform.parent != null && transform.parent.childCount <= 1) 
        {
            Destroy(transform.parent.gameObject);
        }
    }

    void OnMouseDown()
    {
        // Calculate offset of the mouse position and starting position of parent when drag begins
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position;

        transform.parent.GetComponent<Block>().SetSortingLayer(true);
    }

    void OnMouseDrag()
    {
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.parent.position = newPosition - offset;
    }
}
