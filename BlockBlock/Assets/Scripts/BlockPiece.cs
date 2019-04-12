using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPiece : MonoBehaviour
{
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
        if (transform.parent != null && transform.parent.childCount <= 1) {
            Destroy(transform.parent.gameObject);
        }
    }    
}
