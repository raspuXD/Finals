using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorManager : MonoBehaviour
{
    public static ConveyorManager Instance;

    public MoneyManager moneyManager;

    public bool isEquipmentConveyor = false;
    public int BeltLevel = 1;
    public int BeltCost;
    public int MaxBeltLevel = 3;

    public GameObject UpgradeButton;

    public Transform spawnPoint;
    public Transform resetPoint;

    public float MinSpawnInterval = 8f;
    public float MaxSpawnInterval = 12f;
    public float actionCooldown = 3f;

    public List<GameObject> productLevel1Prefabs;
    public List<GameObject> productLevel2Prefabs;
    public List<GameObject> productLevel3Prefabs;

    public List<GameObject> equipmentLevel1Prefabs;
    public List<GameObject> equipmentLevel2Prefabs;
    public List<GameObject> equipmentLevel3Prefabs;

    private Queue<GameObject> itemQueue = new Queue<GameObject>();
    private Queue<GameObject> currentPrefabQueue = new Queue<GameObject>();

    private bool isCooldown = false;
    private float cooldownTimer = 0f;

    void Awake()
    {
        if (!isEquipmentConveyor && Instance == null)
            Instance = this;
    }

    void Start()
    {
        List<GameObject> initialList = GetCurrentPrefabListLegacy();
        foreach (var prefab in initialList)
        {
            if (!currentPrefabQueue.Contains(prefab))
                currentPrefabQueue.Enqueue(prefab);
        }

        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                isCooldown = false;
        }

        UpgradeButton.SetActive(moneyManager.Money >= GetTotalCost() && BeltLevel < MaxBeltLevel);
    }

    IEnumerator SpawnLoop()
    {
        while (isActiveAndEnabled)
        {
            if (!isCooldown)
            {
                HandleNextItem();
                yield return new WaitForSeconds(Random.Range(MinSpawnInterval, MaxSpawnInterval));
            }
            else
            {
                yield return null;
            }
        }
    }

    void HandleNextItem()
    {
        if (itemQueue.Count > 0)
        {
            GameObject item = itemQueue.Dequeue();

            if (item != null)
            {
                item.transform.position = resetPoint.position;

                ConveyorItem conveyorItem = item.GetComponent<ConveyorItem>();
                if (conveyorItem != null)
                {
                    conveyorItem.enabled = false;
                    StartCoroutine(EnableAfterDelay(conveyorItem, 0.75f));
                }

                TryAddToQueue(item);

                StartCooldown();
            }
        }
        else
        {
            SpawnNewItem();
        }
    }

    void SpawnNewItem()
    {
        if (currentPrefabQueue.Count == 0) return;

        GameObject prefab = currentPrefabQueue.Dequeue();
        GameObject newItem = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        ConveyorItem conveyorItem = newItem.GetComponent<ConveyorItem>();
        if (conveyorItem != null)
        {
            conveyorItem.theBelt = this;
        }

        StartCooldown();
    }

    IEnumerator EnableAfterDelay(MonoBehaviour script, float delay)
    {
        yield return new WaitForSeconds(delay);
        script.enabled = true;
    }

    void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = actionCooldown;
    }

    void TryAddToQueue(GameObject item)
    {
        GameObject prefab = item.GetComponent<ConveyorItem>()?.PrefabReference; // You should store the original prefab in the item when instantiating
        if (prefab != null && !currentPrefabQueue.Contains(prefab))
        {
            currentPrefabQueue.Enqueue(prefab);
        }
    }

    List<GameObject> GetCurrentPrefabListLegacy()
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

    public void UpgradeBelt()
    {
        if (moneyManager.Money >= GetTotalCost() && BeltLevel < MaxBeltLevel)
        {
            moneyManager.DecreaseMoney(GetTotalCost());
            BeltLevel++;

            List<GameObject> newLevelPrefabs = GetCurrentPrefabListLegacy();
            foreach (var prefab in newLevelPrefabs)
            {
                if (!currentPrefabQueue.Contains(prefab))
                    currentPrefabQueue.Enqueue(prefab);
            }
        }
    }

    int GetTotalCost() => BeltCost * BeltLevel;

    public void AddItemToQueue(GameObject item)
    {
        if (!itemQueue.Contains(item))
        {
            itemQueue.Enqueue(item);
        }
    }
}
