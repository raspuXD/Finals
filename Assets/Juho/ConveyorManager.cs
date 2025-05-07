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
    private List<GameObject> teleportedItems = new List<GameObject>();

    private int lastSpawnedIndex = -1;
    private bool isCooldown = false;
    private float cooldownTimer = 0f;

    void Awake()
    {
        if (!isEquipmentConveyor && Instance == null)
            Instance = this;
    }

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
            }
        }

        UpgradeButton.SetActive(moneyManager.Money >= GetTotalCost() && BeltLevel < MaxBeltLevel);
    }

    IEnumerator SpawnLoop()
    {
        while (isActiveAndEnabled)
        {
            if (!isCooldown)
            {
                ProcessNextItem();
                yield return new WaitForSeconds(Random.Range(MinSpawnInterval, MaxSpawnInterval));
            }
            else
            {
                yield return null;
            }
        }
    }

    void ProcessNextItem()
    {
        if (itemQueue.Count > 0)
        {
            GameObject item = itemQueue.Dequeue();

            if (item != null)
            {
                if (teleportedItems.Contains(item))
                {
                    TeleportItem(item);
                }
                else
                {
                    item.transform.position = spawnPoint.position;

                    ConveyorItem conveyorItem = item.GetComponent<ConveyorItem>();
                    if (conveyorItem != null)
                    {
                        conveyorItem.enabled = false;
                        StartCoroutine(EnableAfterDelay(conveyorItem, 0.75f));
                    }

                    teleportedItems.Add(item);
                    itemQueue.Enqueue(item);
                    StartCooldown();
                }
            }
        }
        else
        {
            SpawnNewItem();
        }
    }

    void SpawnNewItem()
    {
        List<GameObject> prefabList = GetCurrentPrefabList();
        if (prefabList == null || prefabList.Count == 0) return;

        int index;
        do
        {
            index = Random.Range(0, prefabList.Count);
        } while (index == lastSpawnedIndex && prefabList.Count > 1);

        GameObject newItem = Instantiate(prefabList[index], spawnPoint.position, Quaternion.identity);
        lastSpawnedIndex = index;

        itemQueue.Enqueue(newItem);
        teleportedItems.Add(newItem);
        StartCooldown();
    }

    void TeleportItem(GameObject item)
    {
        item.transform.position = resetPoint.position;

        ConveyorItem conveyorItem = item.GetComponent<ConveyorItem>();
        if (conveyorItem != null)
        {
            conveyorItem.enabled = false;
            StartCoroutine(EnableAfterDelay(conveyorItem, 0.75f));
        }

        itemQueue.Enqueue(item);
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

    public void AddItemToTeleportList(GameObject item)
    {
        if (!itemQueue.Contains(item))
        {
            itemQueue.Enqueue(item);
        }

        if (!teleportedItems.Contains(item))
        {
            teleportedItems.Add(item); // optional tracking
        }
    }


    public void UpgradeBelt()
    {
        if (moneyManager.Money >= GetTotalCost() && BeltLevel < MaxBeltLevel)
        {
            moneyManager.DecreaseMoney(GetTotalCost());
            BeltLevel++;
            Debug.Log($"{(isEquipmentConveyor ? "Equipment" : "Product")} Belt upgraded to level {BeltLevel}.");
        }
    }

    int GetTotalCost() => BeltCost * BeltLevel;
}
