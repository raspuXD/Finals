using UnityEngine;

public class Slot : MonoBehaviour
{
    public Transform scaleTarget;
    public bool isHighlighted = false;
    public float scaleSpeed = 5f;
    private Vector3 originalScale;
    private Vector3 targetScale;
    public bool hasAItemInIt = false;
    public Transform currentItem;
    public Ingredient theIngredient;
    public Seasoning theSeasoning;
    public CookingStyle theCookingStyle;

    public ItemType whatToAcceptToSlot;

    void Start()
    {
        if (scaleTarget == null) scaleTarget = transform;
        originalScale = scaleTarget.localScale;
    }

    void Update()
    {
        if (!hasAItemInIt)
        {
            targetScale = isHighlighted ? originalScale * 1.15f : originalScale;
            scaleTarget.localScale = Vector3.Lerp(scaleTarget.localScale, targetScale, Time.deltaTime * scaleSpeed);
        }
    }

    public void SnapToSlot(Transform item)
    {
        item.position = transform.position;
        item.rotation = transform.rotation;
        currentItem = item;
        if(whatToAcceptToSlot == ItemType.Ingredient)
        {
            theIngredient = currentItem.GetComponent<Ingredient>();
        }
        else if(whatToAcceptToSlot == ItemType.Seasoning)
        {
            theSeasoning = currentItem.GetComponent<Seasoning>();
        }
        else
        {
            theCookingStyle = currentItem.GetComponent<CookingStyle>();
        }
        hasAItemInIt = true;
        scaleTarget.localScale = originalScale;
    }

    public void ClearSlot()
    {
        currentItem.rotation = Quaternion.Euler(0f, 0f, 0f);
        theIngredient = null;
        theSeasoning = null;
        theCookingStyle = null;
        currentItem = null;
        hasAItemInIt = false;
        scaleTarget.localScale = originalScale;
    }
}