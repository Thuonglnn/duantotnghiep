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
            ReloadAfterDelay(5f);
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
        }
        else
        {
            ReloadAfterDelay(5f);
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
        }



    }


    public void LeaveRoom()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown(); // Dừng server và ngắt kết nối


        }
        else if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown(); // Ngắt kết nối khỏi server

        }
    }
    // Hàm để bắt đầu việc load lại với độ trễ
    public void ReloadSceneWithDelay(float delay)
    {
        StartCoroutine(ReloadAfterDelay(delay));
    }

    // Coroutine thực hiện việc chờ
    private IEnumerator ReloadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
