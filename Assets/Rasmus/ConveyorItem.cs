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
        if (other.CompareTag("ResetZone"))
        {
            Transform resetPoint = ConveyorManager.Instance.GetResetPoint();
            if (resetPoint != null)
            {
                transform.position = resetPoint.position;
            }
        }
    }
}
