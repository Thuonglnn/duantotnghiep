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
using UnityEngine.SceneManagement; // Thêm dòng này

public class RoomManager : NetworkBehaviour
{
    // UI Elements
    public TMP_InputField joinRoomInputField;
    public Button createRoomButton;
    public Button joinRoomButton;
    public TextMeshProUGUI joinCodeText;

    PlayerSpawner playerSpawner;



    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        createRoomButton.onClick.AddListener(StartRelay);
        joinRoomButton.onClick.AddListener(JoinRelay);

        playerSpawner = GetComponent<PlayerSpawner>();
    }

    public async void StartRelay()
    {
        string joinCode = await StartHostWithRelay();
        joinCodeText.text = "Join Code: " + joinCode;
        // Chuyển đến scene mới sau khi tạo phòng
        //SceneManager.LoadScene("GameScene"); // Thay "YourNewSceneName" bằng tên scene bạn muốn chuyển đến
    }

    public async void JoinRelay()
    {

        bool joined = await StartClientWithRelay(joinRoomInputField.text);
        if (joined)
        {
            joinCodeText.text = "Joined Room";

            // Chuyển đến scene mới sau khi tham gia phòng
            //SceneManager.LoadScene("GameScene"); // Thay "YourNewSceneName" bằng tên scene bạn muốn chuyển đến
        }
        else
        {
            joinCodeText.text = "Failed to Join Room";

        }


    }

    private async Task<string> StartHostWithRelay(int maxConnections = 4)
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    private async Task<bool> StartClientWithRelay(string joinCode)
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

    public void ReloadCurrentScene()
    {
        // Tải lại scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}