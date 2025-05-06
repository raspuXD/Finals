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

    private int lastSpawnedIndex = -1;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnItem();
            float waitTime = Random.Range(MinSpawnInterval, MaxSpawnInterval);
            Debug.Log(waitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void SpawnItem()
    {
        if (spawnablePrefabs.Count == 0) return;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, spawnablePrefabs.Count);
        } while (newIndex == lastSpawnedIndex && spawnablePrefabs.Count > 1);

        GameObject prefab = spawnablePrefabs[newIndex];
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        lastSpawnedIndex = newIndex;
    }

    public Transform GetResetPoint() => resetPoint;
}
