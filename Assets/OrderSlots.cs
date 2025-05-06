using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSlots : MonoBehaviour
{
    public Ingredient needed1, needed2, needed3;
    public Slot slot1, slot2, slot3;

    bool slot1Correct, slot2Correct, slot3Correct;

    public void Update()
    {
        if(slot1.currentItem != null || slot2.currentItem != null || slot3.currentItem != null)
        {
            Ingredient slot1Ingre = slot1.GetComponent<Ingredient>();
            Ingredient slot2Ingre = slot2.GetComponent<Ingredient>();
            Ingredient slot3Ingre = slot3.GetComponent<Ingredient>();

            if(slot1Ingre != null && slot2Ingre != null && slot3Ingre != null)
            {

            }
        }
    }
}
