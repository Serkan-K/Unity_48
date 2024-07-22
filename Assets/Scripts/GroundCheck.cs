using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float maxDistance = 1;
    public bool ground_Control;

    //protected string object_Tag;
     




    RaycastHit hit;

    void FixedUpdate()
    {

        ground_Control = Physics.BoxCast(transform.position, transform.lossyScale / 2,
            Vector3.down, out hit, transform.rotation, maxDistance);

        //Debug.Log("Is Grounded: " + ground_Control);


        //object_Tag = hit.collider.gameObject.tag;





        if (ground_Control)
        {
            Debug.Log("Temas edilen " + hit.collider.gameObject.name + " " + hit.collider.gameObject.tag);
        }

        //if (object_Tag == "Water")
        //{
        //    //Swim
        //}
        //else if(object_Tag == "Ground")
        //{
        //    //canJump
        //}
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
