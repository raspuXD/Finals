using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private float timer = 0f; // Timer for counting inactive time
    private const float timeToDestroy = 45f; // Time before destruction
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

    void OnMouseUp()
    {
        isDragging = false;

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

    private void Start()
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
    }

    void OnMouseDown()
    {
        if (!hasBeenBough)
        {
            hasBeenBough = true;
            if (whatIsThis == ItemType.Ingredient)
            {
                mane.DecreaseMoney(ingredient.theCost);
            }
            else if (whatIsThis == ItemType.Seasoning)
            {
                mane.DecreaseMoney(seasoning.theCost);
            }
            else
            {
                mane.DecreaseMoney(cookingStyle.theCost);
            }
        }

        beltItem.enabled = false;
        isDragging = true;

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

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mousePosition;

        if (previousSlot != null)
        {
            previousSlot.ClearSlot();
            previousSlot = null;
        }

        // Reset the timer when dragging starts
        timer = 0f;
    }

    private void OnMouseOver()
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
    }

    private void OnMouseExit()
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
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x + offset.x, mousePosition.y + offset.y, transform.position.z);
        }
        else
        {
            // Increment the timer when not dragging
            timer += Time.deltaTime;

            if (timer >= timeToDestroy)
            {
                // Destroy the object after 45 seconds of inactivity
                Destroy(gameObject);
            }
        }
    }
}
