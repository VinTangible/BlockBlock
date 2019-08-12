using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene("Main");
    }

    public void StartGameTimerMode() {
        GameManager.timerMode = true;
        SceneManager.LoadScene("Main");
    }
}
