using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public GameObject playerPrefab, playerPrefab2; // Prefab của nhân vật

    public Transform hostSpawnPoint;   // Điểm spawn của Host
    public Transform clientSpawnPoint; // Điểm spawn của Client
    public TMP_Dropdown tMP_Dropdown;
    public GameObject Char1, Char2, Char3, Char4;
    void Start()
    {

    }
    void Update()
    {


    }
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {

            NetworkManager.OnClientConnectedCallback += OnClientConnected;

        }

        if (IsHost) // Nếu là Host
        {
            CharSelect(tMP_Dropdown.value);

        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} connected.");

        if (clientId != NetworkManager.LocalClientId)
        {
            Debug.Log("Spawning player for Client.");
            SpawnPlayer(clientSpawnPoint.position, clientId);
        }
    }

    private void SpawnPlayer(Vector3 spawnPosition, ulong clientId = 0)
    {
        // Tạo nhân vật tại vị trí đã chọn
        var playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        var networkObject = playerInstance.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            if (clientId == 0)
            {
                clientId = NetworkManager.LocalClientId;
            }

            networkObject.SpawnAsPlayerObject(clientId);
            Debug.Log($"Spawned player object for Client ID: {clientId}");
        }
        else
        {
            Debug.LogError("NetworkObject component not found on playerPrefab.");
        }
    }
    private void SpawnPlayer1(Vector3 spawnPosition, ulong clientId = 0)
    {
        // Tạo nhân vật tại vị trí đã chọn
        var playerInstance = Instantiate(playerPrefab2, spawnPosition, Quaternion.identity);
        var networkObject = playerInstance.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            if (clientId == 0)
            {
                clientId = NetworkManager.LocalClientId;
            }

            networkObject.SpawnAsPlayerObject(clientId);
            Debug.Log($"Spawned player object for Client ID: {clientId}");
        }
        else
        {
            Debug.LogError("NetworkObject component not found on playerPrefab.");
        }
    }

    public void ReloadScript()
    {
        // Gọi lại hàm Start hoặc khởi tạo lại
        Start();
    }


    public void CharSelect(int i)
    {
        switch (i)
        {
            case 0:

                playerPrefab2 = Char1;
                break;
            case 1:
                playerPrefab2 = Char2;
                break;
            case 2:

                playerPrefab2 = Char3;
                break;
            case 3:

                playerPrefab2 = Char4;
                break;
            default:
                break;

        }
    }

    public void CharSelect2(int i)
    {
        switch (i)
        {
            case 0:
                playerPrefab = Char1;

                break;
            case 1:
                playerPrefab = Char2;
                break;
            case 2:
                playerPrefab = Char3;

                break;
            case 3:
                playerPrefab = Char4;

                break;
            default:
                break;

        }
    }


}
