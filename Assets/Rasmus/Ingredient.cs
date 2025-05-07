using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ingredient : MonoBehaviour
{
    public string nameForIngredient;

    public State ingredientType;
    public Sprite image;
    public GameObject theInfoHolder;
    public TMP_Text theCardNameText;
    public int theCost;

    public enum State
    {
        GradedCheese,
        Potato,
        Jauheliha,
        Spagetti,
        Water,
        Tomato,
        CheeseSlice,
        Puns,
        Steak,
        Lemon,
        Salmon,
        Rice
    }

    public void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = image;
        theCardNameText.text = nameForIngredient;
    }
}
