using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public bool timeOrInf;
    public void StartTime(bool gameMode) {
        timeOrInf = gameMode;
        Debug.Log(gameMode);
        Application.LoadLevel("Main");
    }

    public bool getGameMode() {
        return timeOrInf;
    }
}
