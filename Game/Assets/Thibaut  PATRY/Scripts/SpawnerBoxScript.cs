using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBoxScript : MonoBehaviour
{
    public int numberOfBox = 0; 
    public GameObject prefab;


    void Start()
    {
        for (int i = 0; i < numberOfBox; i++)
        {
            prefab.transform.localScale = new Vector3(1,1,1);
            Instantiate(prefab, new Vector3(0, i * 1f, 0), Quaternion.identity);
        }
    }
}
