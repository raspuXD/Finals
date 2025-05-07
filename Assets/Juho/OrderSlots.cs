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

        if(slot1Has &&  slot2Has && slot3Has && slot4Has && slot5Has)
        {
            sendIt.SetActive(true);
        }
        else
        {
            sendIt.SetActive(false);
        }
    }

    void CheckMark()
    {
        if (slot1.theIngredient != null)
        {
            //slot1Has = true;
            if (slot1.theIngredient.ingredientType == theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient1)
            {
                slot1Correct = true;
                checkMark1.SetActive(true);
            }
            else
            {
                slot1Correct = false;
                checkMark1.SetActive(false);
            }
        }
        else
        {
            //slot1Correct = false;
            checkMark1.SetActive(false);
            slot1Has = false;
        }


        if (slot2.theIngredient != null)
        {
            slot2Has = true;
            if (slot2.theIngredient.ingredientType == theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient2)
            {
                slot2Correct = true;
                checkMark2.SetActive(true);
            }
            else
            {
                slot2Correct = false;
                checkMark2.SetActive(false);
            }
        }
        else
        {
            slot2Has = false;
            slot2Correct = false;
            checkMark2.SetActive(false);
        }


        if (slot3.theIngredient != null)
        {
            slot3Has = true;
            if (slot3.theIngredient.ingredientType == theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient3)
            {
                slot3Correct = true;
                checkMark3.SetActive(true);
            }
            else
            {
                slot3Correct = false;
                checkMark3.SetActive(false);
            }
        }
        else
        {
            slot3Has = false;
            slot3Correct = false;
            checkMark3.SetActive(false);
        }

        if (slot4.theSeasoning != null)
        {
            slot4Has = true;
            if (slot4.theSeasoning.seasoningType == theCustomerScript.selectedCustomer.theFoodCustomerWants.seasoning)
            {
                slot4Corrent = true;
                checkMark4.SetActive(true);
            }
            else
            {
                slot4Corrent = false;
                checkMark4.SetActive(false);
            }
        }
        else
        {
            slot4Has = false;
            slot4Corrent = false;
            checkMark4.SetActive(false);
        }

        if (slot5.theCookingStyle != null)
        {
            slot5Has = true;
            if (slot5.theCookingStyle.cookingType == theCustomerScript.selectedCustomer.theFoodCustomerWants.cookingStyle)
            {
                slot5Correct = true;
                checkMark5.SetActive(true);
            }
            else
            {
                slot5Correct = false;
                checkMark5.SetActive(false);
            }
        }
        else
        {
            slot5Has = false;
            slot5Correct = false;
            checkMark5.SetActive(false);
        }
    }
}