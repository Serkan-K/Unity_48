using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float maxDistance = 1;
    public bool isGrounded;
    RaycastHit hit;

    void Update()
    {

        isGrounded = Physics.BoxCast(transform.position, transform.lossyScale / 2,
            Vector3.down, out hit, transform.rotation, maxDistance);


        Debug.Log("Is Grounded: " + isGrounded);
    }

    public void OnDrawGizmos()
    {
        bool isHit = Physics.BoxCast(transform.position, transform.lossyScale / 2,
            Vector3.down, out hit, transform.rotation, maxDistance);

        if (isHit)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position, Vector3.down * maxDistance);
            Gizmos.DrawWireCube(transform.position + Vector3.down * hit.distance, transform.lossyScale / 2);
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, Vector3.down * maxDistance);
        }
    }
}
