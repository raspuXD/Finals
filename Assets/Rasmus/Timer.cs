using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;   // Assign in Inspector
    public float maxTime = 30f;         // Set your max time
    public bool requiredCondition = false;  // Change this from another script when needed
    public bool showTimer = true;       // Controls visibility of timer text

    private float currentTime = 0f;
    private bool hasLost = false;

    void Update()
    {
        // Only run timer if not already failed
        if (!hasLost)
        {
            currentTime += Time.deltaTime;

            // Update text if allowed
            if (timerText != null)
            {
                timerText.gameObject.SetActive(showTimer);
                if (showTimer)
                    timerText.text = Mathf.FloorToInt(currentTime).ToString();
            }

            // Check if time is up
            if (currentTime >= maxTime && !requiredCondition)
            {
                hasLost = true;
                Debug.Log("You lose!");
                // Add your game over logic here
            }
        }
    }
}
