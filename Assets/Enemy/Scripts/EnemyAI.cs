using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask GroundMask, PlayerMask;

    public bool CloseCombat;

    public float health;



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
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, PlayerMask);
        playerInAttRange = Physics.CheckSphere(transform.position, attRange, PlayerMask);

        if (!playerInSightRange && !playerInAttRange) Patroling();
        if (playerInSightRange && !playerInAttRange) ChasePlayer();
        if (playerInSightRange && playerInAttRange) AttPlayer();
    }


    private void Patroling()
    {
        if (!walkpointset) SearchWalkPoint();

        if (walkpointset)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkpointset = false;
        }
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, GroundMask))
        {
            walkpointset = true;
        }

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
        agent.SetDestination(player.position);

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);


    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
