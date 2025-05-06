using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorManager : MonoBehaviour
{
    public static ConveyorManager Instance;
    public MoneyManager moneyManager; // Assign this in the Inspector


    // Current belt level
    public int BeltLevel = 1;
    public int BeltCost;
    public int TotalCost;

    [Header("Spawning")]
    // Separate lists for each level; add or remove levels as needed
    public List<GameObject> level1Prefabs;
    public List<GameObject> level2Prefabs;
    public List<GameObject> level3Prefabs;

    public Transform spawnPoint;
    public Transform resetPoint;
    public float MinSpawnInterval = 8f;
    public float MaxSpawnInterval = 12f;
    public float actionCooldown = 3f;  // Cooldown time in seconds

    private int lastSpawnedIndex = -1;
    private bool isCooldownActive = false;  // Indicates if the cooldown is active
    private float cooldownTimer = 0f;

    // Flags to determine if spawning or teleporting is allowed
    [SerializeField] private bool canSpawn = true;
    private bool canTeleport = true;

    // List to store teleported items
    public List<GameObject> teleportedItems = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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
                isCooldownActive = false;  // Cooldown ended
                canSpawn = true;           // Allow spawning again
                canTeleport = true;        // Allow teleporting again
            }
        }

        if(TotalCost <= moneyManager.Money)
        {
            Debug.Log("MORE MONEY BUT BELT");
        }
    }
    public void UpgradeBelt()
    {
        TotalCost = BeltCost *= BeltLevel;

        moneyManager.Money -= TotalCost;
    }
    IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Only act when cooldown is inactive
            if (!isCooldownActive)
            {
                if (teleportedItems.Count > 0)
                {
                    // Teleport the first item in the list if available
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
                yield return null;  // Wait until cooldown finishes
            }
        }
    }

    void SpawnItem()
    {
        // Select the proper prefab list based on the current belt level
        List<GameObject> currentPrefabs = null;
        if (BeltLevel == 1)
        {
            currentPrefabs = level1Prefabs;
        }
        else if (BeltLevel == 2)
        {
            currentPrefabs = level2Prefabs;
        }
        else if (BeltLevel == 3)
        {
            currentPrefabs = level3Prefabs;
        }
        else
        {
            // Default to level 1 list if the level is undefined
            currentPrefabs = level1Prefabs;
        }

        // Ensure that the selected list contains prefabs
        if (currentPrefabs == null || currentPrefabs.Count == 0)
        {
            Debug.LogWarning("No spawnable prefabs available for BeltLevel " + BeltLevel);
            return;
        }

        // Choose a prefab index ensuring it's not the same as the last spawned (if possible)
        int newIndex;
        do
        {
            newIndex = Random.Range(0, currentPrefabs.Count);
        } while (newIndex == lastSpawnedIndex && currentPrefabs.Count > 1);

        // Instantiate the selected prefab
        GameObject prefab = currentPrefabs[newIndex];
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        lastSpawnedIndex = newIndex;

        // Start the cooldown period after spawning
        StartCooldown();
    }

    public void TeleportItem(GameObject item)
    {
        if (canTeleport && !isCooldownActive)
        {
            item.transform.position = resetPoint.position;

            // Optionally disable movement for a short period
            ConveyorItem conveyorItem = item.GetComponent<ConveyorItem>();
            if (conveyorItem != null)
            {
                conveyorItem.enabled = false;
                StartCoroutine(EnableAfterDelay(conveyorItem, 0.75f));
            }

            teleportedItems.Remove(item);
            StartCooldown();
        }
    }

    IEnumerator EnableAfterDelay(MonoBehaviour script, float delay)
    {
        yield return new WaitForSeconds(delay);
        script.enabled = true;
    }

    void StartCooldown()
    {
        isCooldownActive = true;
        cooldownTimer = actionCooldown;
        canSpawn = false;
        canTeleport = false;
    }

    public Transform GetResetPoint() => resetPoint;

    public void AddItemToTeleportList(GameObject item)
    {
        // Add the item if it's not already in the list
        if (!teleportedItems.Contains(item))
        {
            teleportedItems.Add(item);
        }
    }
}
