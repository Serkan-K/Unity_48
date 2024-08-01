using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gazete : MonoBehaviour
{
    private void OnDisable()
    {
        // Gazete devre dýþý býrakýldýðýnda havuza geri dön
        Object_Pooler.Instance.ReturnToPool(gameObject);
    }

}
