using UnityEngine;

public class ConveyorItem : MonoBehaviour
{
    public float moveSpeed = 2f;

    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with: " + other.name);

        // If the item hits the ResetZone (end of the conveyor), reset it and re-queue it
        if (other.CompareTag("ResetZone"))
        {
            Destroy(gameObject);
        }
    }
}
