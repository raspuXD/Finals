using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderSlots : MonoBehaviour
{
    public Customer theCustomerScript;
    public Slot slot1, slot2, slot3, slot4, slot5;

    public bool slot1Correct, slot2Correct, slot3Correct, slot4Corrent, slot5Correct;
    public bool slot1Has, slot2Has, slot3Has, slot4Has, slot5Has;
    public TMP_Text ingredient1Text, ingredient2Text, ingredient3Text, seasoningText, cookingStyleText;
    public GameObject checkMark1, checkMark2, checkMark3, checkMark4, checkMark5;
    public GameObject sendIt;
    public ParticleSystem ps1, ps2, ps3, ps4, ps5;
    bool lastSlot1Correct, lastSlot2Correct, lastSlot3Correct, lastSlot4Correct, lastSlot5Correct;

    void Update()
    {
        if (theCustomerScript.selectedCustomer.theFoodCustomerWants != null)
        {
            ingredient1Text.text = theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient1.ToString();
            ingredient2Text.text = theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient2.ToString();
            ingredient3Text.text = theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient3.ToString();
            seasoningText.text = theCustomerScript.selectedCustomer.theFoodCustomerWants.seasoning.ToString();
            cookingStyleText.text = theCustomerScript.selectedCustomer.theFoodCustomerWants.cookingStyle.ToString();

            CheckMark();
        }

        if(slot1Has && slot2Has && slot3Has && slot4Has && slot5Has)
        {
            sendIt.SetActive(true);
        }
        else
        {
            sendIt.SetActive(false);
        }
    }

    public void DestroyAllMUHAHAHA()
    {
        if(slot1.currentItem != null)
        {
            Destroy(slot1.currentItem.gameObject);
            Destroy(slot2.currentItem.gameObject);
            Destroy(slot3.currentItem.gameObject);
            Destroy(slot4.currentItem.gameObject);
            Destroy(slot5.currentItem.gameObject);
        }
    }

    void CheckMark()
    {
        if (slot1.theIngredient != null)
        {
            slot1Has = true;
            bool isCorrectNow = slot1.theIngredient.ingredientType == theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient1;

            if (isCorrectNow && !lastSlot1Correct)
                ps1.Play();

            slot1Correct = isCorrectNow;
            lastSlot1Correct = isCorrectNow;
            checkMark1.SetActive(isCorrectNow);
        }
        else
        {
            slot1Correct = false;
            lastSlot1Correct = false;
            checkMark1.SetActive(false);
            slot1Has = false;
        }



        if (slot2.theIngredient != null)
        {
            slot2Has = true;
            bool isCorrectNow = slot2.theIngredient.ingredientType == theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient2;

            if (isCorrectNow && !lastSlot2Correct)
                ps2.Play();

            slot2Correct = isCorrectNow;
            lastSlot2Correct = isCorrectNow;
            checkMark2.SetActive(isCorrectNow);
        }
        else
        {
            slot2Has = false;
            slot2Correct = false;
            lastSlot2Correct = false;
            checkMark2.SetActive(false);
        }



        if (slot3.theIngredient != null)
        {
            slot3Has = true;
            bool isCorrectNow = slot3.theIngredient.ingredientType == theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient3;

            if (isCorrectNow && !lastSlot3Correct)
                ps3.Play();

            slot3Correct = isCorrectNow;
            lastSlot3Correct = isCorrectNow;
            checkMark3.SetActive(isCorrectNow);
        }
        else
        {
            slot3Has = false;
            slot3Correct = false;
            lastSlot3Correct = false;
            checkMark3.SetActive(false);
        }
        if (slot4.theSeasoning != null)
        {
            slot4Has = true;
            bool isCorrectNow = slot4.theSeasoning.seasoningType == theCustomerScript.selectedCustomer.theFoodCustomerWants.seasoning;

            if (isCorrectNow && !lastSlot4Correct)
                ps4.Play();

            slot4Corrent = isCorrectNow;
            lastSlot4Correct = isCorrectNow;
            checkMark4.SetActive(isCorrectNow);
        }
        else
        {
            slot4Has = false;
            slot4Corrent = false;
            lastSlot4Correct = false;
            checkMark4.SetActive(false);
        }
        if (slot5.theCookingStyle != null)
        {
            slot5Has = true;
            bool isCorrectNow = slot5.theCookingStyle.cookingType == theCustomerScript.selectedCustomer.theFoodCustomerWants.cookingStyle;

            if (isCorrectNow && !lastSlot5Correct)
                ps5.Play();

            slot5Correct = isCorrectNow;
            lastSlot5Correct = isCorrectNow;
            checkMark5.SetActive(isCorrectNow);
        }
        else
        {
            slot5Has = false;
            slot5Correct = false;
            lastSlot5Correct = false;
            checkMark5.SetActive(false);
        }

    }
}