using UnityEngine;
using UnityEngine.InputSystem;

public class KeyPickup : MonoBehaviour
{
    public GameObject key;
    private bool hasKey = false;
    private bool isNearKey = false; 
    void Update()
    {
        if (isNearKey && Input.GetKeyDown(KeyCode.G))
        {
            PickUpKey();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearKey = true; 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearKey = false; 
        }
    }

    void PickUpKey()
    {
        hasKey = true;
        key.SetActive(false);
        //Destroy(key);
    }

    public bool HasKey()
    {
        return hasKey;
    }
}
