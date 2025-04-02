using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DayManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] float timer = 120;
    [SerializeField] bool autoDetectZones = true;
    [SerializeField] bool autoStartDay = true;

    [Space]

    [SerializeField] UnityEvent OnCompleted;
    [SerializeField] UnityEvent OnSkillIssue;

    [Header("References")]
    [SerializeField] Text text;
    [SerializeField] GameObject hud;
    [SerializeField] GameObject waypointPrefab;
    [SerializeField] NewDeliveryZone[] zones;

    // Privates
    Timer clock;
    bool completed = false;
    Dictionary<NewDeliveryZone, GameObject> waypoints = new Dictionary<NewDeliveryZone, GameObject>();

    void Start()
    {
        if (autoDetectZones) { zones = FindObjectsOfType<NewDeliveryZone>(); }
        GenerateWaypoints();
        clock = new Timer(timer);
        clock.Reset();
    }

    void GenerateWaypoints()
    {
        foreach (NewDeliveryZone zone in zones)
        {
            GameObject waypoint = Instantiate(waypointPrefab);
            waypoint.transform.SetParent(hud.transform);
            waypoint.GetComponent<UIWaypoint>().target = zone.GetWaypointTransform();
            waypoints.Add(zone, waypoint);
        }
    }

    string ConvertSecondsToTimeString(float time) {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void Update()
    {
        bool temp = true;
        foreach (NewDeliveryZone zone in zones)
        {
            if (zone.Completed())
            {
                GameObject gameObject = waypoints[zone];
                if (gameObject != null) { Destroy(gameObject); }
                Debug.Log(temp);
            }
            else
            {
                temp = false;
                Debug.Log(temp);
                break;
            }
        }
        completed = temp;

        if (completed) { OnCompleted.Invoke(); }

        if(autoStartDay) { clock.Update(); }

        text.text = ConvertSecondsToTimeString(clock.GetTime());

        if (clock.Completed()) { OnSkillIssue.Invoke(); }
    }
}