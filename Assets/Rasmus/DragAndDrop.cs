using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    Ingredient ingredient;
    CookingStyle cookingStyle;
    Seasoning seasoning;
    Slot highlightedSlot;
    Slot currentSlot;
    Slot previousSlot;
    ConveyorItem beltItem;
    [SerializeField] bool hasBeenBough = false;
    public ItemType whatIsThis;
    bool isInMoveZone = false;
    MoneyManager mane;
    SpriteRenderer spriteRenderer;
    Vector3 originalScale;

    void Start()
    {
        if (whatIsThis == ItemType.Ingredient)
        {
            ingredient = GetComponent<Ingredient>();
        }
        else if (whatIsThis == ItemType.Seasoning)
        {
            seasoning = GetComponent<Seasoning>();
        }
        else
        {
            cookingStyle = GetComponent<CookingStyle>();
        }
        beltItem = GetComponent<ConveyorItem>();
        mane = FindObjectOfType<MoneyManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x + offset.x, mousePosition.y + offset.y, transform.position.z);
        }
    }

    void OnMouseDown()
    {
        if (!hasBeenBough)
        {
            hasBeenBough = true;
            if (whatIsThis == ItemType.Ingredient)
            {
                mane.DecreaseMoney(ingredient.theCost);
                PlayFoodSpecificSound(ingredient.ingredientType);
            }
            else if (whatIsThis == ItemType.Seasoning)
            {
                mane.DecreaseMoney(seasoning.theCost);
                PlaySpiceSpecificSound(seasoning.seasoningType);
            }
            else
            {
                mane.DecreaseMoney(cookingStyle.theCost);
                PlayEquipmentSpecificSound(cookingStyle.cookingType);
            }
        }

        beltItem.enabled = false;
        isDragging = true;
        AudioManager.Instance.PlaySFX("CardPickup");

        if (whatIsThis == ItemType.Ingredient)
        {
            ingredient.theInfoHolder.SetActive(false);
            PlayFoodSpecificSound(ingredient.ingredientType);
        }
        else if (whatIsThis == ItemType.Seasoning)
        {
            seasoning.theInfoHolder.SetActive(false);
            PlaySpiceSpecificSound(seasoning.seasoningType);
        }
        else
        {
            cookingStyle.theInfoHolder.SetActive(false);
            PlayEquipmentSpecificSound(cookingStyle.cookingType);
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mousePosition;

        if (previousSlot != null)
        {
            previousSlot.ClearSlot();
            previousSlot = null;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        AudioManager.Instance.PlaySFX("CardPlace");
        if (isInMoveZone)
        {
            beltItem.enabled = true;
        }

        if (currentSlot != null && currentSlot.whatToAcceptToSlot == whatIsThis)
        {
            if (previousSlot != null)
            {
                previousSlot.ClearSlot();
            }

            currentSlot.SnapToSlot(transform);
            currentSlot.isHighlighted = false;
            previousSlot = currentSlot;
        }
        else
        {
            if (highlightedSlot != null)
            {
                highlightedSlot.isHighlighted = false;
            }

            currentSlot = null;
            highlightedSlot = null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Slot slot = other.GetComponent<Slot>();
        if (slot != null && !slot.hasAItemInIt && slot.whatToAcceptToSlot == whatIsThis)
        {
            if (highlightedSlot != null && highlightedSlot != slot)
            {
                highlightedSlot.isHighlighted = false;
            }

            highlightedSlot = slot;
            highlightedSlot.isHighlighted = true;
            currentSlot = slot;
        }

        if (other.CompareTag("MoveZone"))
        {
            isInMoveZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Slot slot = other.GetComponent<Slot>();
        if (slot != null && highlightedSlot == slot)
        {
            highlightedSlot.isHighlighted = false;
            highlightedSlot = null;
            currentSlot = null;
        }

        if (other.CompareTag("MoveZone"))
        {
            isInMoveZone = false;
        }
    }

    void OnMouseOver()
    {
        if (whatIsThis == ItemType.Ingredient)
        {
            ingredient.theInfoHolder.SetActive(true);

            if (!hasBeenBough)
            {
                mane.Hover(-ingredient.theCost);
                ingredient.theCardNameText.text = ingredient.theCost + "€<br>" + ingredient.nameForIngredient;
            }
            else
            {
                ingredient.theCardNameText.text = ingredient.nameForIngredient;
            }
        }
        else if (whatIsThis == ItemType.Seasoning)
        {
            seasoning.theInfoHolder.SetActive(true);

            if (!hasBeenBough)
            {
                mane.Hover(-seasoning.theCost);
                seasoning.theCardNameText.text = seasoning.theCost + "€<br>" + seasoning.nameForSeasoning;
            }
            else
            {
                seasoning.theCardNameText.text = seasoning.nameForSeasoning;
            }
        }
        else
        {
            cookingStyle.theInfoHolder.SetActive(true);

            if (!hasBeenBough)
            {
                mane.Hover(-cookingStyle.theCost);
                cookingStyle.theCardNameText.text = cookingStyle.theCost + "€<br>" + cookingStyle.nameForCookingStyle;
            }
            else
            {
                cookingStyle.theCardNameText.text = cookingStyle.nameForCookingStyle;
            }
        }

        transform.localScale = originalScale * 1.15f;
    }

    void OnMouseExit()
    {
        if (whatIsThis == ItemType.Ingredient)
        {
            ingredient.theInfoHolder.SetActive(false);
        }
        else if (whatIsThis == ItemType.Seasoning)
        {
            seasoning.theInfoHolder.SetActive(false);
        }
        else
        {
            cookingStyle.theInfoHolder.SetActive(false);
        }

        transform.localScale = originalScale;
    }

    private void PlayFoodSpecificSound(Ingredient.State ingredientType)
    {
        switch (ingredientType)
        {
            case Ingredient.State.GratedCheese:
                AudioManager.Instance.PlaySFX("GratedCheese");
                break;
            case Ingredient.State.Potato:
                AudioManager.Instance.PlaySFX("Vegetable");
                break;
            case Ingredient.State.GroundBeef:
                AudioManager.Instance.PlaySFX("Meat");
                break;
            case Ingredient.State.Spaghetti:
                AudioManager.Instance.PlaySFX("Spaghetti");
                break;
            case Ingredient.State.Water:
                AudioManager.Instance.PlaySFX("Water");
                break;
            case Ingredient.State.Tomato:
                AudioManager.Instance.PlaySFX("Vegetable");
                break;
            case Ingredient.State.CheeseSlice:
                AudioManager.Instance.PlaySFX("Cheese");
                break;
            case Ingredient.State.Buns:
                AudioManager.Instance.PlaySFX("CardPickup");
                break;
            case Ingredient.State.Steak:
                AudioManager.Instance.PlaySFX("Meat");
                break;
            case Ingredient.State.Lemon:
                AudioManager.Instance.PlaySFX("Lemon");
                break;
            case Ingredient.State.Salmon:
                AudioManager.Instance.PlaySFX("Meat");
                break;
            case Ingredient.State.Rice:
                AudioManager.Instance.PlaySFX("CardPickup");
                break;
        }
    }

    private void PlayEquipmentSpecificSound(CookingStyle.State cookingType)
    {
        switch (cookingType)
        {
            case CookingStyle.State.Frying:
                AudioManager.Instance.PlaySFX("Frying");
                break;
            case CookingStyle.State.Steam:
                AudioManager.Instance.PlaySFX("Steam");
                break;
            case CookingStyle.State.Grill:
                AudioManager.Instance.PlaySFX("Grill");
                break;
        }
    }

    private void PlaySpiceSpecificSound(Seasoning.State seasoningType)
    {
        switch (seasoningType)
        {
            case Seasoning.State.Plain:
                AudioManager.Instance.PlaySFX("CardPickup");
                break;
            case Seasoning.State.Seasoned:
            case Seasoning.State.Spicy:
                AudioManager.Instance.PlaySFX("Spice");
                break;
        }
    }
}
