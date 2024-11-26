using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TextMeshProUGUI TMP_youWin;
    public TextMeshProUGUI TMP_youLoss;

    RoomManager roomManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        roomManager = GetComponent<RoomManager>();
        TMP_youLoss.text = string.Empty;
        TMP_youWin.text = string.Empty;
    }

    [ServerRpc]
    public void IncreaseScoreServerRpc(bool isHostDead)
    {
        IncreaseScoreClientRpc(isHostDead);
    }
    [ClientRpc]
    public void IncreaseScoreClientRpc(bool isHostDead)
    {
        IncreaseScore(isHostDead);
    }

    public void IncreaseScore(bool isHostDead)
    {
        if (isHostDead)
        {
            
            if (IsHost)
            {
                TMP_youLoss.text = "You Lose!";
                TMP_youWin.text = string.Empty; // Xóa thông báo thắng

            }
            else
            {
                TMP_youWin.text = "You Win!";
                TMP_youLoss.text = string.Empty; // Xóa thông báo thua
            }
            StartCoroutine(ReloadSceneAfterDelay(3));
        }
        else
        {
            
            if (IsHost)
            {
                TMP_youWin.text = "You Win!";
                TMP_youLoss.text = string.Empty; // Xóa thông báo thua
            }
            else
            {
                TMP_youLoss.text = "You Lose!";
                TMP_youWin.text = string.Empty; // Xóa thông báo thắng
            }
            StartCoroutine(ReloadSceneAfterDelay(3));
        }
    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        roomManager.DeleteRoom();
        // Tải lại scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    // public void LeaveRoom()
    // {
    //     if (NetworkManager.Singleton.IsHost)
    //     {
    //         NetworkManager.Singleton.Shutdown(); // Dừng server và ngắt kết nối

    //     }
    //     else if (NetworkManager.Singleton.IsClient)
    //     {
    //         NetworkManager.Singleton.Shutdown(); // Ngắt kết nối khỏi server
    //     }
    // }
    // Hàm để bắt đầu việc load lại với độ trễ
    // Coroutine thực hiện việc chờ

}
