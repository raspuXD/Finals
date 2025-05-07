using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class WhatDidCustomerThink : MonoBehaviour
{
    public Customer customer;
    public OrderSlots orderSlots;

    public string[] ifOnly1WasCorrect, ifOnly2WasCorrect, ifOnly3WasCorrect, ifOnly4WasCorrect, ifAllWere;
    public float timer;
    public bool isRunning = false;
    public UnityEvent onTimerHitTen;

    public int howManyFullyRight = 0;

    public TMP_Text theText;
    private bool hasFiredAtTen = false;
    public FadeIn theFadeIn;

    public TMP_Text howManyRight;

    public void StartWriting()
    {
        int correctCount = 0;
        if (orderSlots.slot1Correct) correctCount++;
        if (orderSlots.slot2Correct) correctCount++;
        if (orderSlots.slot3Correct) correctCount++;
        if (orderSlots.slot4Corrent) correctCount++;
        if (orderSlots.slot5Correct) correctCount++;

        string chosenLine = "";

        if (correctCount == 1 && ifOnly1WasCorrect.Length > 0)
            chosenLine = ifOnly1WasCorrect[Random.Range(0, ifOnly1WasCorrect.Length)];
        else if (correctCount == 2 && ifOnly2WasCorrect.Length > 0)
            chosenLine = ifOnly2WasCorrect[Random.Range(0, ifOnly2WasCorrect.Length)];
        else if (correctCount == 3 && ifOnly3WasCorrect.Length > 0)
            chosenLine = ifOnly3WasCorrect[Random.Range(0, ifOnly3WasCorrect.Length)];
        else if (correctCount == 4 && ifOnly4WasCorrect.Length > 0)
            chosenLine = ifOnly4WasCorrect[Random.Range(0, ifOnly4WasCorrect.Length)];
        else if (correctCount == 5 && ifAllWere.Length > 0)
            chosenLine = ifAllWere[Random.Range(0, ifAllWere.Length)];
        else
            chosenLine = "Hmm... I'm not sure what to say.";

        string finalText = chosenLine + " I wanted my " + customer.selectedCustomer.theFoodCustomerWants.foodName + " to be good!";
        theText.text = finalText;

        if (correctCount == 5)
        {
            howManyFullyRight++;
            howManyRight.text = howManyFullyRight + "/3";

            if (howManyFullyRight == 3)
            {
                theFadeIn.StartFadeIn();
            }
        }
    }


    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;

            if (!hasFiredAtTen && timer >= 7f)
            {
                hasFiredAtTen = true;
                onTimerHitTen.Invoke();
            }
        }
    }

    public void StartTimer()
    {
        timer = 0f;
        isRunning = true;
        hasFiredAtTen = false;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        timer = 0f;
        hasFiredAtTen = false;
    }
}
