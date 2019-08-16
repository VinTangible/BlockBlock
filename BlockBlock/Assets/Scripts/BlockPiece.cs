using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPiece : MonoBehaviour
{
    void OnDestroy()
    {
        // Destroy parent block if this is the last piece left to destroy
        if (transform.parent != null && transform.parent.childCount <= 1) {
            Destroy(transform.parent.gameObject);
        }
    }
}
