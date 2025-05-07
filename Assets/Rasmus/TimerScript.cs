using System.Collections;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public TMP_Text timerText;  // Timer text display
    private float timerDuration = 10f;  // Timer duration in seconds
    private bool isOrderAccepted = false;
    private Customer customer;  // Reference to the Customer script

    private void Start()
    {
        customer = GetComponent<Customer>();  // Get reference to the Customer script
    }

    public void StartTimer()
    {
        isOrderAccepted = true;
        timerText.gameObject.SetActive(true); // Show the timer text
        StartCoroutine(CountdownTimer());
    }

    public void StopTimer()
    {
        isOrderAccepted = false;
        timerText.gameObject.SetActive(false); // Hide the timer text
    }

    public void ResetTimer()
    {
        timerDuration = 0;
    }

    private IEnumerator CountdownTimer()
    {
        float timeRemaining = timerDuration;
        while (timeRemaining > 0)
        {
            timerText.text = "Time left: " + Mathf.Ceil(timeRemaining) + "s";
            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        // Time's up, you lose if order is not served
        if (isOrderAccepted)
        {
            timerText.text = "You Lost!";
            // Handle losing logic (could deactivate customer, show a 'lose' screen, etc.)
            yield return new WaitForSeconds(2f);
            customer.Decline(); // Decline the customer (or handle game-over logic in another way)
        }
    }
}
