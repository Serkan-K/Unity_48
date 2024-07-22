using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float maxDistance = 1;
    public bool ground_Control;

    RaycastHit hit;

    void FixedUpdate()
    {

        ground_Control = Physics.BoxCast(transform.position, transform.lossyScale / 2,
            Vector3.down, out hit, transform.rotation, maxDistance);

        //Debug.Log("Is Grounded: " + ground_Control);
        
        
        if (ground_Control)
        {
            Debug.Log("Deðilen " + hit.collider.gameObject.name + " " + hit.collider.gameObject.tag);
        }
    }


    public void OnDrawGizmos()
    {

        if (ground_Control)
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
