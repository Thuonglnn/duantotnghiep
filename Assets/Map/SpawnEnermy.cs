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

    public int maxEnemies = 20;                 // Số lượng quái tối đa
    private int currentEnemyCount = 0;          // Số lượng quái hiện tại

    private int currentStage = 1;               // Giai đoạn hiện tại
    private bool bossSpawned = false;           // Xác định boss đã spawn hay chưa
    private bool bossDefeated = false;          // Xác định boss đã bị tiêu diệt hay chưa

    private float[] stageThresholds = { 0, 45, 90 }; // Thời gian chuyển giai đoạn

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
            if (currentStage < stageThresholds.Length) // Chưa đến giai đoạn cuối
            {
                SpawnEnemyByStage(); // Spawn quái như bình thường
            }
            else if (!bossSpawned) // Giai đoạn cuối và boss chưa được spawn
            {
                SpawnBoss();
            }

            spawnTimer = spawnInterval;
        }

        UpdateStage();
    }

    void SpawnEnemyByStage()
    {
        if (spawnPoints.Count == 0 || currentEnemyCount >= maxEnemies ||
            (normalEnemies.Count == 0 && eliteEnemies.Count == 0)) return;

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
        }

        if (enemyToSpawn != null)
        {
            SpawnEnemyServerRpc(enemyToSpawn.name, spawnPoint.position, spawnPoint.rotation);
        }
    }

    void SpawnBoss()
    {
        if (spawnPoints.Count == 0 || bossSpawned) return;

        // Chọn ngẫu nhiên một điểm spawn
        int randomSpawnIndex = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[randomSpawnIndex];

        if (bossPrefab != null)
        {
            SpawnEnemyServerRpc(bossPrefab.name, spawnPoint.position, spawnPoint.rotation);
            bossSpawned = true; // Đánh dấu boss đã spawn
            Debug.Log("Boss has spawned!");
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

            currentEnemyCount++; // Tăng số lượng quái hiện tại
            Debug.Log($"Spawned: {prefabName} at {position}. Current enemy count: {currentEnemyCount}");

            // Nếu là boss, gắn sự kiện tiêu diệt đặc biệt
            if (prefab == bossPrefab)
            {
                var bossScript = enemy.AddComponent<Enemy>();
                bossScript.onDestroyed += HandleBossDefeated;
            }
            else
            {
                var enemyScript = enemy.AddComponent<Enemy>();
                enemyScript.onDestroyed += HandleEnemyDestroyed;
            }
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

    private void HandleEnemyDestroyed()
    {
        if (currentEnemyCount > 0) currentEnemyCount--; // Giảm số lượng quái hiện tại
        Debug.Log($"Enemy destroyed. Remaining: {currentEnemyCount}");
    }

    private void HandleBossDefeated()
    {
        bossDefeated = true; // Đánh dấu boss đã bị tiêu diệt
        Debug.Log("Boss defeated! You win!");

        OnPlayerWin();
    }

    void OnPlayerWin()
    {
        Debug.Log("Congratulations! You have defeated the boss!");

        // Logic thêm nếu cần: chuyển cảnh, hiển thị UI chiến thắng
    }

    void UpdateStage()
    {
        for (int i = stageThresholds.Length - 1; i >= 0; i--)
        {
            if (Time.timeSinceLevelLoad >= stageThresholds[i])
            {
                currentStage = i + 1;
                break;
            }
        }
    }
}

// Script gắn cho quái để lắng nghe sự kiện tiêu diệt
public class Enemy : MonoBehaviour
{
    public event System.Action onDestroyed;

    private void OnDestroy()
    {
        onDestroyed?.Invoke(); // Gọi sự kiện khi quái bị phá hủy
    }
}