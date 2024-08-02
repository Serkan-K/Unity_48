using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    [SerializeField] GameObject game_Object;
    [SerializeField] private float delay;

    private void Start()
    {
        StartCoroutine(Activator_Cor());
    }

    private IEnumerator Activator_Cor()
    {
        yield return new WaitForSeconds(delay);
        game_Object.SetActive(false);
    }



    private void OnTriggerEnter(Collider other) { game_Object.SetActive(true); }

    private void OnTriggerExit(Collider other) { game_Object.SetActive(false); }
}
