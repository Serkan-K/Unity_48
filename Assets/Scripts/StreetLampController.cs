using UnityEngine;
using UnityEngine.InputSystem;

public class StreetLampController : MonoBehaviour
{
    [SerializeField] private Light lampLight;
    private bool isLampOn = false;
    private GameObject player;

    public float activationDistance = 2.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lampLight = GetComponentInChildren<Light>();
    }

    public void ToggleLamp()
    {
        isLampOn = !isLampOn;
        lampLight.enabled = isLampOn;
    }

    public bool IsPlayerInRange()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer <= activationDistance;
        }
        return false;
    }
}
