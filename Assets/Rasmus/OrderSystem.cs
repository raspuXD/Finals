using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    public ArrayList[] FoodItems;

    public void CheckList()
    {
        foreach (var item in FoodItems)
        {
            print(item);
        }
    }
}