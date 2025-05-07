using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CookingStyle : MonoBehaviour
{
    public string nameForCookingStyle;

    public State cookingType;
    public Sprite image;
    public GameObject theInfoHolder;
    public TMP_Text theCardNameText;
    public int theCost;

    public enum State
    {
        Grill,
        Steam,
        Frying
    }

    public void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = image;
        theCardNameText.text = nameForCookingStyle;
    }
}
