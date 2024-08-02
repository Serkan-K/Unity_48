using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator : MonoBehaviour
{
    [SerializeField] private GameObject player_;
    [SerializeField] private Vector3 translator_Pos;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 newPos = player_.transform.position;
        newPos = translator_Pos;

        player_.transform.position = newPos;
    }
}
