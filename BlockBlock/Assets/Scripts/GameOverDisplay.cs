using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverDisplay : MonoBehaviour
{
    public GameObject playAgainButton;
    public GameObject mainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.GameOverEvent += OnGameOverEvent;
    }

    private void OnGameOverEvent(object sender, System.EventArgs e)
    {
        playAgainButton.SetActive(true);
        mainMenuButton.SetActive(true);
    }
}