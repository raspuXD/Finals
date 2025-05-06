using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    Ingredient ingredient;
    Slot highlightedSlot;
    Slot currentSlot;
    Slot previousSlot;

    public ItemType whatIsThis;

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
    }


    void OnMouseUp()
    {
        isDragging = false;

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
            // Optional: reset position or provide feedback for invalid drop
        }
    }

    private void Start()
    {
        ingredient = GetComponent<Ingredient>();
    }

    void OnMouseDown()
    {
        isDragging = true;
        ingredient.theInfoHolder.SetActive(false);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mousePosition;

        if (previousSlot != null)
        {
            previousSlot.ClearSlot();
            previousSlot = null;
        }
    }

    private void OnMouseOver()
    {
        ingredient.theInfoHolder.SetActive(true);
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
    }
}
