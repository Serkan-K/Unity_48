using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    public Transform[] waypoints;
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;

    [SerializeField]
    private float speed = 3.5f;

    [SerializeField]
    private float stoppingDistance = 0.5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = false;
        agent.avoidancePriority = Random.Range(30, 70);

        if (waypoints.Length > 0)
        {
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
            else
            {
                Debug.LogError("Ajan þu anda bir NavMesh üzerinde deðil.");
            }
        }
        else
        {
            Debug.LogError("Rota boþ");
        }
    }

    void Update()
    {
        if (waypoints.Length == 0) { return; }

        if (agent.isOnNavMesh)
        {
            if (!agent.pathPending && agent.remainingDistance < stoppingDistance)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
        else
        {
            Debug.LogWarning("Ajan NavMesh üzerinde deðil.");
        }

        //Debug.Log("Agent position: " + agent.transform.position);
        //Debug.Log("Agent destination: " + agent.destination);
        //Debug.Log("Remaining distance: " + agent.remainingDistance);
    }
}
