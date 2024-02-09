using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
 
    public List<GameObject> objects;

    public void SpawnObject(int index)
    {
        Instantiate(objects[index], transform.position, transform.rotation);
    }
}
