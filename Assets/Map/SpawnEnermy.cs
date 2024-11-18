using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

public class EnemySpawner : NetworkBehaviour
{
    public List<GameObject> normalEnemies;      // Danh sách prefab quái thường
    public List<GameObject> eliteEnemies;       // Danh sách prefab quái cao cấp
    public GameObject bossPrefab;               // Prefab boss

    public List<Transform> spawnPoints;         // Danh sách điểm spawn
    public float spawnInterval = 5f;            // Khoảng thời gian giữa các lần spawn
    private float spawnTimer;

    private int currentStage = 1;               // Giai đoạn hiện tại

    void Start()
    {
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        if (!IsServer) return; // Chỉ máy chủ thực thi logic spawn

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            SpawnEnemyByStage(); // Gọi hàm spawn quái theo giai đoạn
            spawnTimer = spawnInterval;
        }

        // Nâng cấp giai đoạn khi đáp ứng điều kiện (có thể dựa trên thời gian hoặc điểm số)
        UpdateStage();
    }

    void SpawnEnemyByStage()
    {
        if (spawnPoints.Count == 0) return;

        // Chọn ngẫu nhiên một điểm spawn
        int randomSpawnIndex = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[randomSpawnIndex];

        GameObject enemyToSpawn = null;

        // Chọn prefab dựa trên giai đoạn
        switch (currentStage)
        {
            case 1:
                enemyToSpawn = normalEnemies[Random.Range(0, normalEnemies.Count)];
                break;
            case 2:
                enemyToSpawn = Random.value > 0.5f
                    ? normalEnemies[Random.Range(0, normalEnemies.Count)]
                    : eliteEnemies[Random.Range(0, eliteEnemies.Count)];
                break;
            case 3:
                if (Random.value > 0.8f)
                    enemyToSpawn = bossPrefab; // Spawn boss với tỉ lệ 20%
                else
                    enemyToSpawn = Random.value > 0.5f
                        ? normalEnemies[Random.Range(0, normalEnemies.Count)]
                        : eliteEnemies[Random.Range(0, eliteEnemies.Count)];
                break;
        }

        if (enemyToSpawn != null)
        {
            // Spawn quái vật trên máy chủ và đồng bộ đến các máy khách
            SpawnEnemyServerRpc(enemyToSpawn.name, spawnPoint.position, spawnPoint.rotation);
        }
    }

    [ServerRpc]
    private void SpawnEnemyServerRpc(string prefabName, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = FindPrefabByName(prefabName);
        if (prefab != null)
        {
            GameObject enemy = Instantiate(prefab, position, rotation);
            enemy.GetComponent<NetworkObject>().Spawn(); // Đồng bộ quái vật trên toàn bộ máy khách
        }
    }

    private GameObject FindPrefabByName(string prefabName)
    {
        foreach (var enemy in normalEnemies)
        {
            if (enemy.name == prefabName) return enemy;
        }
        foreach (var enemy in eliteEnemies)
        {
            if (enemy.name == prefabName) return enemy;
        }
        if (bossPrefab.name == prefabName) return bossPrefab;

        return null;
    }

    void UpdateStage()
    {
        if (Time.timeSinceLevelLoad > 90) currentStage = 3;
        else if (Time.timeSinceLevelLoad > 45) currentStage = 2;
        else currentStage = 1;
    }
}
