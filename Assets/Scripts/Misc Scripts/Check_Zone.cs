using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_Zone : MonoBehaviour
{
    [SerializeField] GameObject player;
    [HideInInspector]
    public Vector3 checkPoint_pos;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            player.transform.position = checkPoint_pos;
    }
}
