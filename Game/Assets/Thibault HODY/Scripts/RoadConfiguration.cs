using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewRoadConfiguration",menuName ="Level Design/Road Configuration")]
public class RoadConfiguration : ScriptableObject
{

    public List<Road> roadElementsList = new List<Road>();

}
