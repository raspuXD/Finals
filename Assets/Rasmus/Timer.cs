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

    private float currentTime;
    private bool hasLost = false;
    public bool canBeDone = false;

    private bool timerRunning = false;
    public FadeIn theLose;

    void Update()
    {
        if (!hasLost && timerRunning)
        {
            currentTime -= Time.deltaTime;

            if (timerText != null)
            {
                timerText.gameObject.SetActive(showTimer);
                if (showTimer)
                    timerText.text = Mathf.CeilToInt(currentTime).ToString() + "S";
            }

            if (currentTime <= 0f && !requiredCondition)
            {
                hasLost = true;
                timerRunning = false;
                currentTime = 0f; // Clamp to 0 to avoid negative values in UI
                if (timerText != null)
                {
                    timerText.text = "0";
                }
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
        currentTime = maxTime;
        hasLost = false;
        timerRunning = true;
    }

    public void StopAndResetTimer()
    {
        currentTime = maxTime;
        hasLost = false;
        timerRunning = false;

        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(currentTime).ToString();
        }
    }
}
