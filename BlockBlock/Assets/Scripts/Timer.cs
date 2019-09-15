using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private string TIMER_LABEL = "Time: ";
    private IEnumerator coroutine;


    public int startTime = 5;   // Default start time is set to 5 if it's not set
    public int timeLeft;        // Time left
    public Text timeDisplay;   // Text object to display time left

    void Awake() {
        GameManager.instance.GameOverEvent += OnGameOverEvent;
        coroutine = Countdown();

        SetTime(startTime);
    }

    // Add or subtract time left
    public void AddTime(int delta)
    {
        timeLeft += delta;
        UpdateTimerText();
    }

    // Set time left
    public void SetTime(int time)
    {
        timeLeft = time;
        UpdateTimerText();
    }

    // Sets the Text display for the time left
    public void SetDisplay(bool display)
    {
        timeDisplay.gameObject.SetActive(display);
    }

    // Starts the countdown
    public void StartTimer()
    {
        StartCoroutine(coroutine);
    }

    // Pauses the countdown
    public void StopTimer()
    {
        StopCoroutine(coroutine);
    }

    // Stops the timer on Game Over
    private void OnGameOverEvent(object sender, System.EventArgs e)
    {
        StopTimer();
    }

    // Decrements time every second and fires Game Over when time reaches 0
    private IEnumerator Countdown()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            AddTime(-1);
        }

        GameManager.instance.GameOver();
    }

    private void UpdateTimerText()
    {
        timeDisplay.text = TIMER_LABEL + timeLeft.ToString();
    }
}