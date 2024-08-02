using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    public KeyPickup keyPickup;
    private bool isOpen = false; 
    private bool isNearChest = false;

    void Update()
    {
        if (isNearChest && keyPickup.HasKey() && Input.GetKeyDown(KeyCode.O) && !isOpen)
        {
            OpenChest(); 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearChest = true; 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearChest = false; 
        }
    }

    void OpenChest()
    {
        isOpen = true;
        // Buraya fonsiyon veya animasyon koyaabiliriz serkan.
        Debug.Log("Sandýk açýldý!");
    }
}
