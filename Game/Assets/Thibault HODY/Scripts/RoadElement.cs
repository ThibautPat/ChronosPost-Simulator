using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadElement : MonoBehaviour
{
    public Transform[] points;
    public GameObject[] meshes;

    public void Bake()
    {
        foreach (GameObject go in meshes)
        {
            go.transform.parent = null;
        }
        meshes = null;
        foreach (Transform go in points) {

            DestroyImmediate(go.gameObject);
        } 
        points = null;
        DestroyImmediate(this.gameObject);
    }
}
