using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] // Allows private variables to show in Inspector
    private Text scoreText;
    private int score;

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
