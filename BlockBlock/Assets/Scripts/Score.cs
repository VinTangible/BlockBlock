using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "";
    }

    // Update is called once per frame
    private void Update()
    {
        scoreText.text = GameManager.instance.getScore().ToString();
    }
}
