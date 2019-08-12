using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int startTime = 5;
    public int timeLeft;
    public Text timeDisplay;

    private string TIMER_LABEL = "Time: ";

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.timerMode)
        {
            timeLeft = startTime;
            SetTimerText();
            StartCoroutine(Countdown());
        }
        else
        {
            timeDisplay.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    private IEnumerator Countdown()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
            SetTimerText();
        }

        GameManager.instance.GameOver();
    }

    private void SetTimerText()
    {
        timeDisplay.text = TIMER_LABEL + timeLeft.ToString();
    }
}
