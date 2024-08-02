using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float maxDistance = .5f;
    public float rayOffset = 1;
    private bool _hit;
    public bool _walk, _swim, _fall;

    RaycastHit hit;

    void FixedUpdate()
    {
        Vector3 rayOrigin = transform.position;
        rayOrigin.y += rayOffset;

        _hit = Physics.BoxCast(rayOrigin, transform.lossyScale / 2,
            Vector3.down, out hit, transform.rotation, maxDistance);

        if (_hit)
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                //canJump;
                _walk = true;
                _swim = false;
                _fall = false;
            }
            else if (hit.collider.gameObject.CompareTag("Water"))
            {
                _swim = true;
                _fall = false;
                _walk = false;
            }
        }
        else
        {
            _fall = true;
            _walk = false;
            _swim = false;
        }
    }

    public void OnDrawGizmos()
    {

        Vector3 rayOrigin = transform.position;
        rayOrigin.y += rayOffset;

        if (_hit)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(rayOrigin, Vector3.down * maxDistance);
            Gizmos.DrawWireCube(rayOrigin + Vector3.down * hit.distance, transform.lossyScale / 2);
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(rayOrigin, Vector3.down * maxDistance);
        }
    }
}
