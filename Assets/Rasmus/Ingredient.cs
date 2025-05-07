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
        GratedCheese,
        Potato,
        GroundBeef,
        Spaghetti,
        Water,
        Tomato,
        CheeseSlice,
        Buns,
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
