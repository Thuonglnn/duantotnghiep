using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class SpiderQueenAI : MonoBehaviour
{
    // Start is called before the first frame update

    public NavMeshAgent agent;

    Transform player;

    public LayerMask GroundMask, PlayerMask;

    public bool CloseCombat;

    public float health;

    Animator anim;

    // Tuần tra = Patroling

    public Vector3 walkPoint;
    bool walkpointset;
    public float walkPointRange;

    // Chiến đấu = Att

    public float attDelay;
    bool att;
    public GameObject projectile;

    // Giai đoạn = States

    public float sightRange, attRange;
    public bool playerInSightRange, playerInAttRange;


    // private void Awake()
    // {
    //     player = GameObject.Find("PlayerObj").transform;
    //     agent = GetComponent<NavMeshAgent>();
    // }

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        if (health > 0)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, PlayerMask);
            playerInAttRange = Physics.CheckSphere(transform.position, attRange, PlayerMask);

            if (!playerInSightRange && !playerInAttRange)
            {
                anim.SetBool("PlayerInSignR", false);
                Patroling();
            }
            if (playerInSightRange && !playerInAttRange)
            {
                anim.SetBool("PlayerInAttR", false);
                ChasePlayer();
            }
            if (playerInSightRange && playerInAttRange) AttPlayer();
        }


    }


    private void Patroling()
    {
        if (!walkpointset) Invoke(nameof(SearchWalkPoint), 2.5f);

        if (walkpointset)
        {

            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkpointset = false;
            anim.SetTrigger("IsIdle");
        }
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, GroundMask))
        {
            Invoke(nameof(DelayWalk), 10f);
        }
    }
    private void DelayWalk()
    {
        walkpointset = true;
        anim.SetTrigger("Walk");
    }
    private void AttPlayer()
    {
        // Cho quái không di chuyển
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!att)
        {
            /// Att 


            if (CloseCombat)
            {
                // Cận chiến
                anim.SetBool("PlayerInAttR", true);
            }
            else
            {
                // Tầm xa
                Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            }
            ///
            att = true;

            Invoke(nameof(ResetAtt), attDelay);
        }
    }
    private void ResetAtt()
    {
        att = false;

    }
    private void ChasePlayer()
    {
        anim.SetBool("PlayerInSignR", true);
        agent.SetDestination(player.position);

    }

    public void TakeDamage(float damage, float CritRate, float CritDamage)
    {

        if (Random.Range(0f, 1f) <= CritRate)
        {
            damage *= CritDamage;
            DamagePopUp.damagePopUp.createPopUp(new Vector3(gameObject.transform.position.x,
                 gameObject.transform.position.y + Random.Range(0.4f, 0.8f),
                gameObject.transform.position.z - 0.2f), damage + "", Color.red);
            health -= damage;
        }
        else
        {
            DamagePopUp.damagePopUp.createPopUp(new Vector3(gameObject.transform.position.x,
                 gameObject.transform.position.y + Random.Range(0.4f, 0.8f),
                gameObject.transform.position.z - 0.2f), damage + "", Color.white);
            health -= damage;
        }

        if (health <= 0)
        {
            anim.SetBool("PlayerInAttR", false);
            anim.SetBool("PlayerInSignR", false);
            anim.SetBool("Die", true);
            Invoke(nameof(DestroyEnemy), 2.5f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}

