using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void PlayAgain() {
        Application.LoadLevel("Main");
    }
}
