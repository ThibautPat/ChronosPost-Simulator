using UnityEngine;
using UnityEngine.Events;

public class NewDeliveryZone : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] bool autoDetectUnloaders = true;

    [Space]

    [SerializeField] UnityEvent OnCompleted;

    [Header("References")]
    [SerializeField] Transform waypoint;
    [SerializeField] DeliveryUnloader[] unloaders;

    // Privates
    bool completed = false;
    bool cleanup = false;

    public bool Completed() { return completed; }
    public Transform GetWaypointTransform() { return waypoint; }

    void Start() { if (autoDetectUnloaders) { unloaders = GetComponentsInChildren<DeliveryUnloader>(); } }

    void Update()
    {
        if (cleanup) { return; }

        bool temp = true;
        foreach (DeliveryUnloader unloader in unloaders)
        {
            if(!unloader.completed) { temp = false; break; }
        }
        completed = temp; if (completed)
        {
            OnCompleted.Invoke();
            Destroy(waypoint.gameObject);
            cleanup = true;
        }
    }
}