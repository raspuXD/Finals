using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorManager : MonoBehaviour
{
    public static ConveyorManager Instance;

    public MoneyManager moneyManager;

    public int BeltLevel = 1;
    public int BeltCost;
    public int MaxBeltLevel = 3;

    public GameObject UpgradeButton;

    public Transform productSpawnPoint; // Spawn point for products
    public Transform equipmentSpawnPoint; // Spawn point for equipment

    public float MinSpawnInterval = 8f;
    public float MaxSpawnInterval = 12f;
    public float actionCooldown = 3f;

    public List<GameObject> productLevel1Prefabs;
    public List<GameObject> productLevel2Prefabs;
    public List<GameObject> productLevel3Prefabs;

    public List<GameObject> equipmentLevel1Prefabs;
    public List<GameObject> equipmentLevel2Prefabs;
    public List<GameObject> equipmentLevel3Prefabs;

    private bool isCooldown = false;
    private float cooldownTimer = 0f;

    private GameObject lastSpawnedProduct = null;
    private GameObject lastSpawnedEquipment = null;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void OnEnable()
    {
        // Restart the coroutine when the object is enabled
        StartCoroutine(SpawnLoop());
    }

    void OnDisable()
    {
        // Optionally stop the coroutine if the object is disabled
        StopAllCoroutines();
    }

    void Update()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                isCooldown = false;
        }

        UpgradeButton.SetActive(moneyManager.Money >= BeltCost * BeltLevel && BeltLevel < MaxBeltLevel);
    }

    IEnumerator SpawnLoop()
    {
        while (isActiveAndEnabled)
        {
            if (!isCooldown)
            {
                SpawnNewItem();
                yield return new WaitForSeconds(Random.Range(MinSpawnInterval, MaxSpawnInterval));
            }
            else
            {
                yield return null;
            }
        }
    }

    void SpawnNewItem()
    {
        // Spawn product
        List<GameObject> productPrefabs = GetProductPrefabList();
        if (productPrefabs.Count > 0)
        {
            GameObject productPrefab = GetRandomItem(productPrefabs, lastSpawnedProduct);
            Instantiate(productPrefab, productSpawnPoint.position, Quaternion.identity);
            lastSpawnedProduct = productPrefab;
        }

        // Spawn equipment
        List<GameObject> equipmentPrefabs = GetEquipmentPrefabList();
        if (equipmentPrefabs.Count > 0)
        {
            GameObject equipmentPrefab = GetRandomItem(equipmentPrefabs, lastSpawnedEquipment);
            Instantiate(equipmentPrefab, equipmentSpawnPoint.position, Quaternion.identity);
            lastSpawnedEquipment = equipmentPrefab;
        }

        StartCooldown();
    }

    GameObject GetRandomItem(List<GameObject> prefabs, GameObject lastSpawned)
    {
        // Ensure we don't spawn the same item twice in a row
        GameObject randomPrefab;
        do
        {
            randomPrefab = prefabs[Random.Range(0, prefabs.Count)];
        } while (randomPrefab == lastSpawned);

        return randomPrefab;
    }

    void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = actionCooldown;
    }

    List<GameObject> GetProductPrefabList()
    {
        switch (BeltLevel)
        {
            case 1: return productLevel1Prefabs;
            case 2: return productLevel2Prefabs;
            case 3: return productLevel3Prefabs;
            default: return productLevel1Prefabs;
        }
    }

    List<GameObject> GetEquipmentPrefabList()
    {
        switch (BeltLevel)
        {
            case 1: return equipmentLevel1Prefabs;
            case 2: return equipmentLevel2Prefabs;
            case 3: return equipmentLevel3Prefabs;
            default: return equipmentLevel1Prefabs;
        }
    }

    public void UpgradeBelt()
    {
        if (moneyManager.Money >= BeltCost * BeltLevel && BeltLevel < MaxBeltLevel)
        {
            moneyManager.DecreaseMoney(BeltCost * BeltLevel);
            BeltLevel++;

            UpgradeButton.SetActive(moneyManager.Money >= BeltCost * BeltLevel && BeltLevel < MaxBeltLevel);
        }
    }
}
