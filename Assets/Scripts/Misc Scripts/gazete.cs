using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gazete : MonoBehaviour
{
    private void OnDisable()
    {
        // Gazete devre d��� b�rak�ld���nda havuza geri d�n
        Object_Pooler.Instance.ReturnToPool(gameObject);
    }

}
