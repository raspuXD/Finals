using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorManager : MonoBehaviour
{
    public static ConveyorManager Instance;

    [Header("Spawning")]
    public List<GameObject> spawnablePrefabs;
    public Transform spawnPoint;
    public Transform resetPoint;
    public float MinSpawnInterval = 8f;
    public float MaxSpawnInterval = 12f;
    public float actionCooldown = 3f;  // 3 seconds cooldown

    private int lastSpawnedIndex = -1;
    private bool isCooldownActive = false;  // Flag to check if cooldown is active
    private float cooldownTimer = 0f;

    // Flags to determine if spawning or teleporting is allowed
    [SerializeField] bool canSpawn = true;
    private bool canTeleport = true;

    // List to store teleported items
    public List<GameObject> teleportedItems = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
        if (isCooldownActive)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isCooldownActive = false;  // Reset the cooldown state
                canSpawn = true;  // Allow spawning again after cooldown
                canTeleport = true;  // Allow teleporting again after cooldown
            }
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Only spawn or teleport if cooldown is not active
            if (!isCooldownActive)
            {
                if (teleportedItems.Count > 0) // There are items to teleport
                {
                    // Only teleport one item at a time
                    TeleportItem(teleportedItems[0]);
                }
                else
                {
                    SpawnItem();
                }

                float waitTime = Random.Range(MinSpawnInterval, MaxSpawnInterval);
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                yield return null;  // Wait until cooldown is over
            }
        }
    }

    void SpawnItem()
    {
        int newIndex;
        do
        {
            newIndex = Random.Range(0, spawnablePrefabs.Count);
        } while (newIndex == lastSpawnedIndex && spawnablePrefabs.Count > 1);

        GameObject prefab = spawnablePrefabs[newIndex];
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        lastSpawnedIndex = newIndex;

        // Start cooldown after spawning an item
        StartCooldown();
    }

    public void TeleportItem(GameObject item)
    {
        if (canTeleport && !isCooldownActive)
        {
            item.transform.position = resetPoint.position;

            // Remove the teleported item from the list after teleporting
            teleportedItems.Remove(item);

            // Start cooldown after teleporting an item
            StartCooldown();
        }
    }

    void StartCooldown()
    {
        isCooldownActive = true;
        cooldownTimer = actionCooldown;  // Reset the cooldown timer

        // Block both spawning and teleporting during cooldown
        canSpawn = false;
        canTeleport = false;
    }

    public Transform GetResetPoint() => resetPoint;

    public void AddItemToTeleportList(GameObject item)
    {
        // Add item to the teleport list if it is not already in the list
        if (!teleportedItems.Contains(item))
        {
            teleportedItems.Add(item);
        }
    }
}
