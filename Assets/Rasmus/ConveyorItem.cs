using UnityEngine;

public class ConveyorItem : MonoBehaviour
{
    public float moveSpeed = 2f;
    public ConveyorManager theBelt;
    public GameObject PrefabReference;

    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with: " + other.name);

        // Check if the item collided with a "ResetZone"
        if (other.CompareTag("ResetZone"))
        {
            // Add the item to the teleport list in ConveyorManager
            theBelt.AddItemToQueue(gameObject);
        }
    }
}
