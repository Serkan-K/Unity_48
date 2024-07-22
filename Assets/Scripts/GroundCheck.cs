using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float maxDistance = 1;
    public bool ground_Contr;

    RaycastHit hit;

    void FixedUpdate()
    {

        ground_Contr = Physics.BoxCast(transform.position, transform.lossyScale / 2,
            Vector3.down, out hit, transform.rotation, maxDistance);


        //Debug.Log("Is Grounded: " + ground_Contr);
    }


    public void OnDrawGizmos()
    {

        if (ground_Contr)
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
