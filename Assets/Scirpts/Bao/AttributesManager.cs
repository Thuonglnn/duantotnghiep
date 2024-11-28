using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;


public class AttributesManager : NetworkBehaviour
{
    public NetworkVariable<int> hp = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<bool> isdie = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public int def = 100;
    public int atk = 10;
    public float critRate = 0.5f; // 50%
    public float critDamage = 2f; // 200%

    Animator animator;

    ScoreManager scoreManager;
    //ok

    public AttributesManager attributesManager;
    public AttributesManager attributesManagerEnemy;


    public Slider healthBar;

    bool isDie = false;

    public NetworkObject player;
    SpiderQueenAI spiderQueenAI;



    // private void Awake() {
    //     DontDestroyOnLoad(gameObject);
    // }



    private void Start()
    {
        spiderQueenAI = GetComponent<SpiderQueenAI>();
        animator = GetComponent<Animator>();
        scoreManager = GetComponent<ScoreManager>();
        if (IsOwner && healthBar != null)
        {
            healthBar.maxValue = hp.Value;
            healthBar.minValue = 0;
        }

    }

    private void Update()
    {
        if (IsOwner && healthBar != null)
        {
            healthBar.value = hp.Value;

            if (attributesManager)
            {
                if (attributesManager.hp.Value <= 0 && !isDie)
                {
                    isDie = true;
                    isdie.Value = true;
                    UpdateAnimationStateServerRpc("Death", isdie.Value);
                    //gameObject.SetActive(false);
                    ScoreManager.Instance.IncreaseScoreServerRpc(IsHost);
                    //ReloadSceneWithDelay(5f);
                }
                else
                {
                    isdie.Value = false;
                    UpdateAnimationStateServerRpc("Death", isdie.Value);
                }
            }

        }

        if (hp.Value <= 0)
        {

            if (attributesManagerEnemy)
            {
                Destroy(gameObject);
            }

        }

    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Tải lại scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    [ServerRpc(RequireOwnership = false)]
    public void DealDmgServerRpc(NetworkObjectReference targetRef, int attack)
    {
        if (targetRef.TryGet(out NetworkObject targetObj))
        {
            var targetAttributes = targetObj.GetComponent<AttributesManager>();

            if (targetAttributes != null)
            {
                bool isCrit = Random.value <= critRate;
                int finalDamage = isCrit ? Mathf.RoundToInt(attack * critDamage) : attack;

                targetAttributes.TakeDmg(finalDamage, isCrit);
            }
        }
    }

    public void DealDmg(GameObject target, int attack)
    {
        if (IsOwner)
        {
            NetworkObjectReference targetRef = target.GetComponent<NetworkObject>();
            DealDmgServerRpc(targetRef, attack);
        }
    }

    public void TakeDmg(int amount, bool isCrit)
    {
        if (IsServer)
        {
            int damage = Mathf.Max(0, amount - def);
            hp.Value -= damage;

            if (!isCrit)
            {
                DmgPopUpGerenator.Instance.CreatePopUp(transform.position, damage.ToString(), Color.red, false);
            }
            else
            {
                DmgPopUpGerenator.Instance.CreatePopUp(transform.position, damage.ToString(), Color.yellow, true);
            }
        }
    }

    [ServerRpc]
    void UpdateAnimationStateServerRpc(string parameter, bool state)
    {
        UpdateAnimationStateClientRpc(parameter, state);
    }

    [ClientRpc]
    void UpdateAnimationStateClientRpc(string parameter, bool state)
    {
        animator.SetBool(parameter, state);
    }


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
