using System.Collections.Generic;
using UnityEngine;

public class Object_Pooler : MonoBehaviour
{
    public static Object_Pooler Instance;
    public GameObject pooledObjectPrefab;
    public Transform poolParent;
    public int poolSize;

    private List<GameObject> pooledObjects;

    private void Awake() { Instance = this; }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < poolSize; i++) { CreatePooledObject(); }
    }

    private GameObject CreatePooledObject()
    {
        GameObject obj = Instantiate(pooledObjectPrefab, poolParent);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeInHierarchy) { return obj; }
        }


        if (pooledObjects.Count >= poolSize)
        {
            GameObject oldestObject = pooledObjects[0];

            pooledObjects.RemoveAt(0);
            oldestObject.SetActive(false);
            pooledObjects.Add(oldestObject);

            return oldestObject;
        }

        return CreatePooledObject();
    }


    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(poolParent);
    }


}
