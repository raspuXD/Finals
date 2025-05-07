using System.Collections;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public int Money;                       // Changed from float to int
    public TextMeshProUGUI moneyText;

    public float fadeDelay = 3f;            // Time before fading starts
    public float fadeDuration = 1f;         // Duration of fade
    private float lastChangeTime;
    private CanvasGroup canvasGroup;
    public ConveyorManager belt;
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

    public void DecreaseMoney(int amount)   // Changed from float to int
    {
        Money -= amount;
        if (Money <= 0)
        {
            Money = 0; // Prevent negative money
            NoMoney();
        }
        TriggerMoneyChange(true);
    }

    public void IncreaseMoney(int amount)
    {
        Money += amount;
        TriggerMoneyChange(false);
        moneyText.text = Money.ToString() + "€   + " + amount.ToString() + "€";
        lastChangeTime = Time.time;
        canvasGroup.alpha = 1f;

        StartCoroutine(ShowNormalTextAfterDelay(2f));
    }

    IEnumerator ShowNormalTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateMoneyDisplay();
    }


    void TriggerMoneyChange(bool wannaUpdate)
    {
        if(wannaUpdate)
        {
            UpdateMoneyDisplay();
        }
        lastChangeTime = Time.time;
        canvasGroup.alpha = 1f; // Reset fade on activity
    }

    void UpdateMoneyDisplay()
    {
        moneyText.text = Money.ToString() + "€";
    }

    public void HoverBeltButton()
    {
        TriggerMoneyChange(false);

        moneyText.text = Money.ToString() + "€   -" + belt.BeltCost.ToString() + "€";
    }

    public void NoMoney()
    {
        Debug.Log("GameOver");
        SceneHandler Scene = FindAnyObjectByType<SceneHandler>();
        Scene.LoadSceneNamed("lostallmoney");
    }

    // Hover method to show the amount after hover
    public void Hover(int amount)
    {
        TriggerMoneyChange(false);

        if(amount < 0)
        {
            moneyText.text = Money.ToString() + "€   " + amount.ToString() + "€";
        }
        else
        {
            moneyText.text = Money.ToString() + "€   + " + amount.ToString() + "€";
        }
    }
}
