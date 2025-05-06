using System.Collections;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public float Money;
    public TextMeshProUGUI moneyText;

    public float fadeDelay = 3f;         // Time before fading starts
    public float fadeDuration = 1f;      // Duration of fade
    private float lastChangeTime;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (moneyText == null)
        {
            Debug.LogError("MoneyText is not assigned.");
            enabled = false;
            return;
        }

        canvasGroup = moneyText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = moneyText.gameObject.AddComponent<CanvasGroup>();
        }

        UpdateMoneyDisplay();
    }

    void Update()
    {
        float timeSinceChange = Time.time - lastChangeTime;

        if (timeSinceChange > fadeDelay)
        {
            float fadeAmount = 1f - ((timeSinceChange - fadeDelay) / fadeDuration);
            canvasGroup.alpha = Mathf.Clamp01(fadeAmount);
        }
    }

    public void DecreaseMoney(float amount)
    {
        Money -= amount;
        if (Money <= 0)
        {
            NoMoney();
        }
        TriggerMoneyChange();
    }

    public void IncreaseMoney(float amount)
    {
        Money += amount;
        TriggerMoneyChange();
    }

    void TriggerMoneyChange()
    {
        UpdateMoneyDisplay();
        lastChangeTime = Time.time;
        canvasGroup.alpha = 1f; // Reset fade on activity
    }

    void UpdateMoneyDisplay()
    {
        moneyText.text = "$" + Money.ToString("F2");
    }

    public void NoMoney()
    {
        Debug.Log("GameOver");
    }
}
