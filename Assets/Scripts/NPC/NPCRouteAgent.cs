/*using UnityEngine;
using UnityEngine.AI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class NPCRouteAgent : Agent
{
    public Transform[] waypoints; // Hedef noktalar�
    private int currentWaypointIndex = 0;
    private NavMeshAgent navMeshAgent;

    public LayerMask interactableLayer;

    public override void Initialize()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetNextDestination();
    }

    public override void OnEpisodeBegin()
    {
        // NPC'yi rastgele bir konuma yerle�tir
        transform.position = waypoints[Random.Range(0, waypoints.Length)].position;
        SetNextDestination();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Mevcut konum ve hedef waypoint aras�ndaki mesafeyi g�zle
        sensor.AddObservation(Vector3.Distance(transform.position, navMeshAgent.destination));
        sensor.AddObservation(navMeshAgent.destination - transform.position);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Hedefe ula��ld� m�?
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 1.0f)
        {
            InteractWithEnvironment();
            SetNextDestination();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Bo� b�rak�ld�
    }

    private void SetNextDestination()
    {
        // Mevcut waypoint'ten bir sonraki waypoint'e ge�
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    private void InteractWithEnvironment()
    {
        Collider[] interactables = Physics.OverlapSphere(transform.position, 1.0f, interactableLayer);
        foreach (var interactable in interactables)
        {
            // Etkile�im mant��� burada i�lenir
            Debug.Log("Interacted with: " + interactable.gameObject.name);
        }
    }
}
*/