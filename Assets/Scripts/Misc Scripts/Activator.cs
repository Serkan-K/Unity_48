using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    [SerializeField] GameObject game_Object;

    private void Start() { game_Object.SetActive(false); }

    private void OnTriggerEnter(Collider other) { game_Object.SetActive(true); }

    private void OnTriggerExit(Collider other) { game_Object.SetActive(false); }
}
