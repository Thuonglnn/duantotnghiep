using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TextMeshProUGUI TMP_youWin;
    public TextMeshProUGUI TMP_youLoss;
    public GameObject panelScore;
    public TextMeshProUGUI timeOut;
    public float countdownTime = 10f;
    public GameObject esc;
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
        timeOut.text = string.Empty;
        panelScore.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            esc.SetActive(true);
        }
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
            
            StartCoroutine(CountdownTimer());
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
            
            StartCoroutine(CountdownTimer());
        }
    }

        public void IncreaseScoreWin()
        {
            TMP_youWin.text = "You Win!";
            TMP_youLoss.text = string.Empty;
                
            StartCoroutine(CountdownTimer());
        
            
        }

        public void ESC()
        {
                
            StartCoroutine(reLoadScen());
        
        }




    public IEnumerator CountdownTimer()
    {
        float currentTime = countdownTime;
        panelScore.SetActive(true);
        while (currentTime > 0)
        {
            // Cập nhật đối tượng TextMeshProUGUI với thời gian còn lại
            timeOut.text = currentTime.ToString("F2"); // Định dạng 2 chữ số thập phân
            yield return new WaitForSeconds(1f); // Chờ 1 giây
            currentTime--;
        }

        // Khi đếm ngược kết thúc, gọi coroutine reload
        yield return StartCoroutine(ReloadSceneAfterDelay(countdownTime)); // Truyền một độ trễ nếu cần
    }

    


    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        yield return StartCoroutine(roomManager.DeleteRoom());
        // Tải lại scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        panelScore.SetActive(false);
    }

    private IEnumerator reLoadScen()
    {
        yield return StartCoroutine(roomManager.DeleteRoom());
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
