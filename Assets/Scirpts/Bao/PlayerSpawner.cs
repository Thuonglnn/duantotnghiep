using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : NetworkBehaviour
{
    public GameObject Char1, Char2, Char3, Char4; // Các nhân vật
    public Button[] characterButtons; // Danh sách các nút chọn nhân vật
    public Button startButton;        // Nút "Bắt đầu"

    public Transform hostSpawnPoint;   // Điểm spawn cho Host
    public Transform clientSpawnPoint; // Điểm spawn cho Client

    private GameObject selectedCharacter; // Nhân vật được chọn (chỉ local)
    private NetworkVariable<int> selectedCharacterIndex = new NetworkVariable<int>(0); // Chỉ số nhân vật (sync qua network)

    GameManager gameManager;
    bool mode = false;
    void Start()
    {
        gameManager = GetComponent<GameManager>();

        if (gameManager.dropdownGameMode != null)
        {
            gameManager.dropdownGameMode.onValueChanged.AddListener(OnGameModeChanged);
            OnGameModeChanged(gameManager.dropdownGameMode.value); // Kiểm tra giá trị ban đầu
        }
        // Đăng ký sự kiện cho các nút chọn nhân vật
        if (characterButtons != null && characterButtons.Length > 0)
        {
            for (int i = 0; i < characterButtons.Length; i++)
            {
                int index = i; // Đảm bảo closure không bị sai
                characterButtons[i].onClick.AddListener(() => OnCharacterButtonClicked(index));
            }
        }

        // Gắn sự kiện cho nút "Bắt đầu"
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            startButton.gameObject.SetActive(false); // Ẩn nút ban đầu
        }
    }


    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} connected.");
    }

    private void OnCharacterButtonClicked(int index)
    {
        // Lưu nhân vật được chọn vào biến local
        selectedCharacter = GetCharacterPrefab(index);

        if (selectedCharacter != null)
        {
            Debug.Log($"Selected character {index}");
            if (startButton != null)
            {
                startButton.gameObject.SetActive(true); // Hiển thị nút "Bắt đầu"
            }
        }
    }

    private void OnStartButtonClicked()
    {
        if (selectedCharacter == null)
        {
            Debug.LogError("No character selected!");
            return;
        }

        Debug.Log("Start button clicked. Sending character to server...");

        // Gửi thông tin nhân vật đã chọn lên server
        int characterIndex = GetCharacterIndex(selectedCharacter);
        SelectCharacterServerRpc(characterIndex);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SelectCharacterServerRpc(int characterIndex, ServerRpcParams rpcParams = default)
    {
        Debug.Log($"Server received character index: {characterIndex}");

        // Spawn nhân vật cho client
        ulong clientId = rpcParams.Receive.SenderClientId;
        Transform spawnPoint = clientId == NetworkManager.ServerClientId ? hostSpawnPoint : clientSpawnPoint;
        SpawnPlayer(spawnPoint.position, clientId, characterIndex);
    }

    private void SpawnPlayer(Vector3 spawnPosition, ulong clientId, int characterIndex)
    {
        GameObject prefabToSpawn = GetCharacterPrefab(characterIndex);

        if (prefabToSpawn == null)
        {
            Debug.LogError("Invalid character index. Spawning default character.");
            return;
        }

        GameObject playerInstance = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();

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

    private int GetCharacterIndex(GameObject character)
    {
        if (character == Char1) return 0;
        if (character == Char2) return 1;
        if (character == Char3) return 2;
        if (character == Char4) return 3;
        return -1; // Không hợp lệ
    }

    void OnGameModeChanged(int modeValue)
    {
        string newTag = modeValue == 0 ? "Player1" : "Player";

        // Gắn tag cho Host
        SetCharactersTag(newTag);

        // Nếu là Host, đồng bộ tag đến Client
        if (IsServer)
        {
            SyncCharacterTagsClientRpc(newTag);
            Debug.Log($"Host set all characters to tag '{newTag}' and synced with clients.");
        }
        // if (modeValue == 0) // Game Mode là 0
        // {
        //     SetCharactersTag("Player1");
        //     Debug.Log("Game Mode 0: Set all characters to tag 'Player1'.");
        // }
        // else
        // {
        //     SetCharactersTag("Untagged"); // Hoặc tag khác tùy bạn
        //     Debug.Log($"Game Mode {modeValue}: Reset character tags.");
        // }
    }

    void SetCharactersTag(string tag)
    {
        if (Char1 != null) Char1.tag = tag;
        if (Char2 != null) Char2.tag = tag;
        if (Char3 != null) Char3.tag = tag;
        if (Char4 != null) Char4.tag = tag;
    }

    [ClientRpc]
    void SyncCharacterTagsClientRpc(string tag)
    {
        SetCharactersTag(tag);
    }

}
