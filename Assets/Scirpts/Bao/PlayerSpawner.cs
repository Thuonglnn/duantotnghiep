using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public GameObject playerPrefab, playerPrefab2; // Prefab của nhân vật

    public Transform hostSpawnPoint;   // Điểm spawn của Host
    public Transform clientSpawnPoint; // Điểm spawn của Client

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.OnClientConnectedCallback += OnClientConnected;
        }

        if (IsHost) // Nếu là Host
        {
            Debug.Log("Spawning player for Host.");
            //SpawnPlayer(hostSpawnPoint.position);
            SpawnPlayer1(hostSpawnPoint.position);
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


}
