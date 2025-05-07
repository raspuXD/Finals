using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float maxTime = 30f;
    public bool requiredCondition = false;
    public bool showTimer = true;

    private float currentTime = 0f;
    private bool hasLost = false;
    public bool canBeDone = false;

    private bool timerRunning = false;
    public FadeIn theLose;

    void Update()
    {
        if (!hasLost && timerRunning)
        {
            currentTime += Time.deltaTime;

            if (timerText != null)
            {
                timerText.gameObject.SetActive(showTimer);
                if (showTimer)
                    timerText.text = Mathf.FloorToInt(currentTime).ToString();
            }

            if (currentTime >= maxTime && !requiredCondition)
            {
                hasLost = true;
                timerRunning = false;
                theLose.StartFadeIn();
            }
        }
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void StartTimer()
    {
        currentTime = 0f;
        hasLost = false;
        timerRunning = true;
    }

    public void StopAndResetTimer()
    {
        currentTime = 0f;
        hasLost = false;
        timerRunning = false;

        if (timerText != null)
        {
            timerText.text = "0";
        }
    }

}
