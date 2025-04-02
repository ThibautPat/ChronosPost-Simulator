using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceSound : MonoBehaviour
{
    [Tooltip("Area of the sound to be in")]
    public Collider Area;
    [Tooltip("Character to track")]
    public GameObject Player;

    void Update()
    {
        Vector3 closestPoint = Area.ClosestPoint(Player.transform.position);
        transform.position = closestPoint;
    }
}