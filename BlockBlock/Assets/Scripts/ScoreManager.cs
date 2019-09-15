using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int score;

    public Text scoreText;

    private void Awake()
    {
        Score = 0;
    }

    // This public var Score updates private var score when set and also updates text
    public int Score
    {
        get {return score;}
        set {
            score = value;
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
