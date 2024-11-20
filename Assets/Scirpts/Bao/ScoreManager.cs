using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private NetworkVariable<int> hostScore = new NetworkVariable<int>(0);
    private NetworkVariable<int> clientScore = new NetworkVariable<int>(0);

    public TextMeshProUGUI TMP_hostScore;
    public TextMeshProUGUI TMP_clientScore;

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
    
    private void Update()
    {
        if (TMP_hostScore != null)
        {
            TMP_hostScore.text = "" + hostScore.Value;
        }

        if (TMP_clientScore != null)
        {
            TMP_clientScore.text = "" + clientScore.Value;
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void IncreaseScoreServerRpc(bool isHost)
    {
        IncreaseScore(isHost);
    }

    public void IncreaseScore(bool isHost)
    {
        if (isHost)
        {
            hostScore.Value++;
        }
        else
        {
            clientScore.Value++;
        }
    }
}
