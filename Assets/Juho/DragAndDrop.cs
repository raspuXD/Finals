using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private float timer = 0f; // Timer for counting inactive time
    private const float timeToDestroy = 45f; // Time before destruction
    Ingredient ingredient;
    Slot highlightedSlot;
    Slot currentSlot;
    Slot previousSlot;
    ConveyorItem beltItem;
    [SerializeField] bool hasBeenBough = false;
    public ItemType whatIsThis;
    bool isInMoveZone = true;
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
        ingredient = GetComponent<Ingredient>();
        beltItem = GetComponent<ConveyorItem>();
        mane = FindObjectOfType<MoneyManager>();
    }

    void OnMouseDown()
    {
        if (!hasBeenBough)
        {
            hasBeenBough = true;
            mane.DecreaseMoney(ingredient.theCost);
        }

        beltItem.enabled = false;
        isDragging = true;
        ingredient.theInfoHolder.SetActive(false);

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

    private void OnMouseExit()
    {
        ingredient.theInfoHolder.SetActive(false);
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
