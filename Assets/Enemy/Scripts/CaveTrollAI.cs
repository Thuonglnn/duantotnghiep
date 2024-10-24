using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class CaveTrollAI : MonoBehaviour
{
    // Start is called before the first frame update

    public NavMeshAgent agent;

    private Transform player;

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

    public float sightRange, attRange, sightRange2;
    public bool playerInSightRange, playerInAttRange;

    // private void Awake()
    // {
    //     player = GameObject.Find("PlayerObj").transform;
    //     agent = GetComponent<NavMeshAgent>();
    // }

    public float timeLeft = 10.0f;
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
                anim.SetBool("PlayerInAttR", false);
                anim.SetBool("PlayerInSignR", false);
                //Patroling();
            }
            if (playerInSightRange && !playerInAttRange)
            {
                anim.SetBool("PlayerInAttR", false);
                sightRange2 = 9999f;
                playerInSightRange = Physics.CheckSphere(transform.position, sightRange2, PlayerMask);
                ChasePlayer();
            }
            if (playerInSightRange && playerInAttRange)
            {
                AttPlayer();
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0)
                {
                    anim.SetTrigger("HeavyAtt");
                    timeLeft = 10f;
                }
            }
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

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            anim.SetTrigger("Die");
            Invoke(nameof(DestroyEnemy), 10f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }



}


