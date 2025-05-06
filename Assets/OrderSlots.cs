using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderSlots : MonoBehaviour
{
    public Customer theCustomerScript;
    public WholeFood theCurrentOne;
    public Slot slot1, slot2, slot3;

    public bool slot1Correct, slot2Correct, slot3Correct;
    public TMP_Text theOrderText;

    void Update()
    {
        if(theCurrentOne == null)
        {
            theCurrentOne = theCustomerScript.selectedCustomer.theFoodCustomerWants;

            theOrderText.text = theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient1.ToString() + "<br>"
                + theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient2.ToString()+ "<br>" +
                theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient3.ToString();
        }

        if(slot1.theIngredient != null)
        {
            if (slot1.theIngredient.ingredientType == theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient1)
            {
                slot1Correct = true;
            }
            else
            {
                slot1Correct = false;
            }
        }
        else
        {
            slot1Correct = false;
        }


        if (slot2.theIngredient != null)
        {
            if (slot2.theIngredient.ingredientType == theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient2)
            {
                slot2Correct = true;
            }
            else
            {
                slot2Correct = false;
            }
        }
        else
        {
            slot2Correct = false;
        }


        if (slot3.theIngredient != null)
        {
            if (slot3.theIngredient.ingredientType == theCustomerScript.selectedCustomer.theFoodCustomerWants.ingredient3)
            {
                slot3Correct = true;
            }
            else
            {
                slot3Correct = false;
            }
        }
        else
        {
            slot3Correct = false;
        }
    }
}