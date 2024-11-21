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

    // Biến lưu lựa chọn nhân vật của client
    private NetworkVariable<int> selectedCharacter = new NetworkVariable<int>(0);

    void Start()
    {
        if (tMP_Dropdown != null)
        {
            // Đăng ký sự kiện OnValueChanged của Dropdown
            tMP_Dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
    }

    void Update() { }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.OnClientConnectedCallback += OnClientConnected;
        }

        if (IsHost) // Nếu là Host
        {
            CharSelect(tMP_Dropdown.value); // Host chọn nhân vật từ dropdown
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} connected.");
        if (clientId != NetworkManager.LocalClientId)
        {
            // Đợi client gửi lựa chọn nhân vật
            Debug.Log("Waiting for client character selection...");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SelectCharacterServerRpc(int characterIndex, ServerRpcParams rpcParams = default)
    {
        // Lưu lựa chọn nhân vật cho client
        selectedCharacter.Value = characterIndex;

        // Spawn nhân vật tại điểm spawn của client
        SpawnPlayer(clientSpawnPoint.position, rpcParams.Receive.SenderClientId, characterIndex);
    }

    private void SpawnPlayer(Vector3 spawnPosition, ulong clientId, int characterIndex)
    {
        // Chọn prefab dựa trên lựa chọn nhân vật
        GameObject prefabToSpawn = GetCharacterPrefab(characterIndex);

        if (prefabToSpawn == null)
        {
            Debug.LogError("Invalid character index. Spawning default character.");
            prefabToSpawn = playerPrefab;
        }

        var playerInstance = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        var networkObject = playerInstance.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.SpawnAsPlayerObject(clientId);
            Debug.Log($"Spawned character {characterIndex} for Client ID: {clientId}");
        }
        else
        {
            Debug.LogError("NetworkObject component not found on character prefab.");
        }
    }

    private GameObject GetCharacterPrefab(int index)
    {
        switch (index)
        {
            case 0: return Char1;
            case 1: return Char2;
            case 2: return Char3;
            case 3: return Char4;
            default: return null;
        }
    }

    public void CharSelect(int i)
    {
        playerPrefab2 = GetCharacterPrefab(i);
    }

    public void OnDropdownValueChanged(int value)
    {
        if (IsClient)
        {
            // Gửi lựa chọn nhân vật lên server
            SelectCharacterServerRpc(value);
        }
    }
}
