using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderSlots : MonoBehaviour
{
    public Customer theCustomerScript;
    public Slot slot1, slot2, slot3;

    public bool slot1Correct, slot2Correct, slot3Correct;
    public TMP_Text ingredient1Text, ingredient2Text, ingredient3Text;
    public GameObject checkMark1, checkMark2, checkMark3;

    void Update()
    {
        if(theCustomerScript.selectedCustomer.theFoodCustomerWants != null)
        {
            ingredient1Text.text = theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient1.ToString();
            ingredient2Text.text = theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient2.ToString();
            ingredient3Text.text = theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient3.ToString();
        }

        if(slot1.theIngredient != null)
        {
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
            slot1Correct = false;
            checkMark1.SetActive(false);
        }


        if (slot2.theIngredient != null)
        {
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
            slot2Correct = false;
            checkMark2.SetActive(false);
        }


        if (slot3.theIngredient != null)
        {
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
            slot3Correct = false;
            checkMark3.SetActive(false);
        }
    }
}