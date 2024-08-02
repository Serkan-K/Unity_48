using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private GameObject pipe;

    private Rigidbody rb;
    private BoxCollider collider;

    private void Awake()
    {
        rb = pipe.GetComponent<Rigidbody>();
        collider = pipe.GetComponent<BoxCollider>();
        rb.useGravity = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rb.useGravity = true;
            collider.enabled = false;
        }
    }
}
