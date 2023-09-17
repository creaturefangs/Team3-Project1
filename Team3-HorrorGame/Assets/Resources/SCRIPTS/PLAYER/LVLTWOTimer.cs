using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LVLTWOTimer : MonoBehaviour
{

    public TMP_Text timerText;
    private float startTime;
    private float countdownTime = 180.0f; // 3 minutes
    private bool isTimerRunning = false;
    public PlayerHealth playerHealth;
    public GameObject timerTXT;

    private void Start()
    {
        startTime = Time.time;
        isTimerRunning = true;
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            float currentTime = countdownTime - (Time.time - startTime);
            if (currentTime < 0)
            {
                currentTime = 0;
                isTimerRunning = false;
            }

            string minutes = ((int)currentTime / 60).ToString("00");
            string seconds = (currentTime % 60).ToString("00");

            timerText.text = $"{minutes}:{seconds}";
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
        timerTXT.SetActive(true);
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        //playerHealth.Die();
        Debug.Log("Player will die.");
    }
}


