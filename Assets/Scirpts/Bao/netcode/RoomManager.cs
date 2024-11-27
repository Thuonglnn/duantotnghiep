using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Unity.Services.Relay.Models;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class RoomManager : NetworkBehaviour
{
    // UI Elements
    public TMP_InputField joinRoomInputField;
    public Button createRoomButton;
    public Button joinRoomButton;
    public TextMeshProUGUI joinCodeText;
    public TextMeshProUGUI Notification;

    public GameObject joinRoom;
    public GameObject unJoinRoom;

    public TMP_Dropdown tMP_DropdownMap;
    public TMP_Dropdown tMP_DropdownGameMode;
    PlayerSpawner playerSpawner;

    string MapName;
    string GameMode;

    void SwitchName()
    {
        switch (tMP_DropdownMap.value)
        {
            case 0: MapName = "Đấu trường"; break;
            case 1: MapName = "Rừng Thông"; break;
            default: break;
        }
        switch (tMP_DropdownGameMode.value)
        {
            case 0: GameMode = "PVP"; break;
            case 1: GameMode = "PVE"; break;
            default: break;
        }
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        createRoomButton.onClick.AddListener(StartRelay);
        joinRoomButton.onClick.AddListener(JoinRelay);

        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;

        playerSpawner = GetComponent<PlayerSpawner>();
    }

    private void Update()
    {
        SwitchName();
    }

    public void SetActiveButton()
    {
        unJoinRoom.SetActive(false);
        joinRoom.SetActive(true);
    }

    public async void StartRelay()
    {
        string joinCode = await StartHostWithRelay();
        joinCodeText.text = "Mã phòng: " + joinCode;
        CreateRoomPost(joinCode, MapName, GameMode);
        SetActiveButton();
        Notification.text = "Vào phòng thành công";
    }

    public async void JoinRelay()
    {
        bool joined = await StartClientWithRelay(joinRoomInputField.text);
        if (joined)
        {
            joinCodeText.text = "Mã phòng : " + joinRoomInputField.text;
            Notification.text = "Vào phòng thành công";
            SetActiveButton();
        }
        else
        {
            Notification.text = "Vào phòng thất bại";
        }
    }

    private async Task<string> StartHostWithRelay(int maxConnections = 4)
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    public async Task<bool> StartClientWithRelay(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            return NetworkManager.Singleton.StartClient();
        }
        catch
        {
            return false;
        }
    }

    public IEnumerator DeleteRoom()
    {
        // Xử lý khi ứng dụng thoát
        if (IsHost)
        {
            string joinCode = joinCodeText.text.Replace("Mã phòng: ", "").Trim();
            yield return StartCoroutine(DeleteRoomPost(joinCode));
        }
    }

    private void OnApplicationQuit()
    {
        // Xử lý khi ứng dụng thoát
        if (IsHost)
        {
            string joinCode = joinCodeText.text.Replace("Mã phòng: ", "").Trim();
            StartCoroutine(DeleteRoomPost(joinCode));
        }
        ReloadCurrentScene();
    }

    private void OnClientDisconnect(ulong clientId)
    {
        // Kiểm tra nếu client ngắt kết nối là chủ phòng
        if (IsHost && clientId == NetworkManager.Singleton.LocalClientId)
        {
            string joinCode = joinCodeText.text.Replace("Mã phòng: ", "").Trim();
            StartCoroutine(DeleteRoomPost(joinCode));

        }
        else
        {
            ReloadCurrentScene();
        }
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CreateRoomPost(string RoomId, string MapName, string GameMode)
    {
        string url = "http://localhost:3005/RoomId/createroomid";
        var roomData = new
        {
            roomId = RoomId,
            mapName = MapName,
            gameMode = GameMode
        };

        string jsonData = JsonConvert.SerializeObject(roomData);
        StartCoroutine(PostRequest(url, jsonData));
    }

    private IEnumerator PostRequest(string url, string jsonData)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Phản hồi từ server: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Lỗi: " + request.error);
        }
    }

    private IEnumerator DeleteRoomPost(string RoomId)
    {
        string url = $"http://localhost:3005/RoomID/deleteroom/{RoomId}";

        UnityWebRequest request = UnityWebRequest.Delete(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Đã xóa phòng {RoomId} thành công.");
        }
        else
        {
            Debug.LogError($"Lỗi khi xóa phòng {RoomId}: {request.error}");
        }
    }
}
