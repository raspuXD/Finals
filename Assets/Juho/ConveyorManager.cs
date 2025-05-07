using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorManager : MonoBehaviour
{
    public static ConveyorManager Instance;
    public MoneyManager moneyManager;

    [Header("General Settings")]
    public bool isEquipmentConveyor = false; // Toggle this in the inspector per conveyor type
    public int BeltLevel = 1;
    private int MaxBeltLevel = 3;
    public int BeltCost;
    public int TotalCost;
    public GameObject UpgradeButton;

    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public Transform resetPoint;
    public float MinSpawnInterval = 8f;
    public float MaxSpawnInterval = 12f;
    public float actionCooldown = 3f;

    [Header("Product Line Prefabs")]
    public List<GameObject> productLevel1Prefabs;
    public List<GameObject> productLevel2Prefabs;
    public List<GameObject> productLevel3Prefabs;

    [Header("Equipment Line Prefabs")]
    public List<GameObject> equipmentLevel1Prefabs;
    public List<GameObject> equipmentLevel2Prefabs;
    public List<GameObject> equipmentLevel3Prefabs;

    private int lastSpawnedIndex = -1;
    private bool isCooldownActive = false;
    private float cooldownTimer = 0f;

    [SerializeField] private bool canSpawn = true;
    private bool canTeleport = true;

    public List<GameObject> teleportedItems = new List<GameObject>();

    void Awake()
    {
        if (!isEquipmentConveyor && Instance == null)
        {
            Instance = this;
        }
    }


    void Start()
    {
        TotalCost = BeltCost * BeltLevel;
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
        if (isCooldownActive)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isCooldownActive = false;
                canSpawn = true;
                canTeleport = true;
            }
        }

        TotalCost = BeltCost * BeltLevel;

        if (moneyManager.Money >= TotalCost && BeltLevel < MaxBeltLevel)
        {
            UpgradeButton.SetActive(true);
        }
        else
        {
            UpgradeButton.SetActive(false);
        }
    }

    public void UpdateUpgradeButtonState()
    {
        TotalCost = BeltCost * BeltLevel;
        UpgradeButton.SetActive(moneyManager.Money >= TotalCost && BeltLevel < MaxBeltLevel);
    }

    public void UpgradeBelt()
    {
        if (moneyManager.Money >= TotalCost && BeltLevel < MaxBeltLevel)
        {
            moneyManager.DecreaseMoney(TotalCost);
            BeltLevel++;
            TotalCost = BeltCost * BeltLevel;
            Debug.Log($"{(isEquipmentConveyor ? "Equipment" : "Product")} Belt upgraded to level {BeltLevel}. Next cost: {TotalCost}");
            UpdateUpgradeButtonState();
        }
    }

    IEnumerator SpawnLoop()
    {
        while (isActiveAndEnabled)
        {
            if (!isCooldownActive)
            {
                if (teleportedItems.Count > 0)
                {
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
                yield return null;
            }
        }
    }

    void SpawnItem()
    {
        List<GameObject> currentPrefabs = GetCurrentPrefabList();

        if (currentPrefabs == null || currentPrefabs.Count == 0)
        {
            Debug.LogWarning("No prefabs for this conveyor and level.");
            return;
        }

        int newIndex;
        do
        {
            newIndex = Random.Range(0, currentPrefabs.Count);
        } while (newIndex == lastSpawnedIndex && currentPrefabs.Count > 1);

        GameObject prefab = currentPrefabs[newIndex];
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        lastSpawnedIndex = newIndex;

        StartCooldown();
    }

    List<GameObject> GetCurrentPrefabList()
    {
        if (isEquipmentConveyor)
        {
            return BeltLevel switch
            {
                1 => equipmentLevel1Prefabs,
                2 => equipmentLevel2Prefabs,
                3 => equipmentLevel3Prefabs,
                _ => equipmentLevel1Prefabs
            };
        }
        else
        {
            return BeltLevel switch
            {
                1 => productLevel1Prefabs,
                2 => productLevel2Prefabs,
                3 => productLevel3Prefabs,
                _ => productLevel1Prefabs
            };
        }
    }

    public void TeleportItem(GameObject item)
    {
        if (canTeleport && !isCooldownActive)
        {
            item.transform.position = resetPoint.position;

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
        TotalCost = BeltCost * BeltLevel;
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
        if (!teleportedItems.Contains(item))
        {
            teleportedItems.Add(item);
        }
    }
}
