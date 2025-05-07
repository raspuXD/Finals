using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Seasoning : MonoBehaviour
{
    public string nameForSeasoning;

    public State seasoningType;
    public Sprite image;
    public GameObject theInfoHolder;
    public TMP_Text theCardNameText;
    public int theCost;

    public enum State
    {
        Plain,
        Seasoned,
        Spicy
    }

    public void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = image;
        theCardNameText.text = nameForSeasoning;
    }
}
