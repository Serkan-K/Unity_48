using UnityEngine;
using UnityEngine.AI;

public class ThiefController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float chaseRange = 10f;
    [SerializeField] private float stopChaseRange = 15f;
    [SerializeField] private float thiefSpeed = 3.5f; // H�rs�z�n h�z�n� ayarlamak i�in
    [SerializeField] private Transform baseCenter; // H�rs�z�n base merkezi
    [SerializeField] private float baseRadius = 10f; // Base'in yar��ap�
    [SerializeField] private float sneakChaseMultiplier = 0.5f; // Sneak durumundaki alg�lama mesafesi �arpan�

    private NavMeshAgent agent;
    private bool isChasing = false;
    private bool isReturning = false;
    private Vector3 randomDestination;
    private PlayerController playerController;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = thiefSpeed; // H�rs�z�n h�z�n� ayarl�yoruz

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        playerController = player.GetComponent<PlayerController>();

        SetRandomDestination();
    }

    void Update()
    {
        float currentChaseRange = chaseRange;
        /*if (playerController != null && playerController.IsSneaking())
        {
            currentChaseRange *= sneakChaseMultiplier;
        }
        */

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < currentChaseRange)
        {
            isChasing = true;
            isReturning = false;
        }
        else if (distanceToPlayer > stopChaseRange)
        {
            isChasing = false;
            isReturning = true;
        }

        if (isChasing)
        {
            agent.SetDestination(player.position);
        }
        else if (isReturning)
        {
            if (Vector3.Distance(transform.position, baseCenter.position) > baseRadius)
            {
                agent.SetDestination(baseCenter.position);
            }
            else
            {
                SetRandomDestination();
            }
        }
        else
        {
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                SetRandomDestination();
            }
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * baseRadius;
        randomDirection += baseCenter.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, baseRadius, 1);
        randomDestination = hit.position;
        agent.SetDestination(randomDestination);
    }
}
