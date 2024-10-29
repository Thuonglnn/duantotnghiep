using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
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
        spawnTimer = spawnInterval;             // Đặt thời gian chờ ban đầu
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;           // Giảm thời gian chờ theo thời gian thực

        if (spawnTimer <= 0)
        {
            SpawnEnemyByStage();                // Gọi hàm spawn quái theo giai đoạn
            spawnTimer = spawnInterval;         // Đặt lại thời gian chờ
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

        // Spawn theo giai đoạn
        switch (currentStage)
        {
            case 1:  // Giai đoạn 1: chỉ spawn quái thường
                enemyToSpawn = normalEnemies[Random.Range(0, normalEnemies.Count)];
                break;
            case 2:  // Giai đoạn 2: spawn quái thường và quái cao cấp
                enemyToSpawn = Random.value > 0.5f ?
                               normalEnemies[Random.Range(0, normalEnemies.Count)] :
                               eliteEnemies[Random.Range(0, eliteEnemies.Count)];
                break;
            case 3:  // Giai đoạn 3: spawn boss cùng các quái cao cấp

                if (Random.value > 0.8f)
                    enemyToSpawn = bossPrefab;   // Spawn boss với tỉ lệ 20%
                else
                    enemyToSpawn = Random.value > 0.5f ?
                               normalEnemies[Random.Range(0, normalEnemies.Count)] :
                               eliteEnemies[Random.Range(0, eliteEnemies.Count)];
                break;
        }

        // Spawn quái được chọn
        Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity); //spawnPoint.rotation
    }

    void UpdateStage()
    {
        // Điều kiện để nâng cấp giai đoạn (có thể dựa trên thời gian hoặc điểm số)
        // Ví dụ nâng cấp giai đoạn sau mỗi 30 giây
        if (Time.timeSinceLevelLoad > 90) currentStage = 3;
        else if (Time.timeSinceLevelLoad > 45) currentStage = 2;
        else currentStage = 1;
    }
}
