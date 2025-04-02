using UnityEngine;

public class WorldWaypoint : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] float multiplier = 1.2f;
    [SerializeField] float range = 8.6f;

    // Privates
    Material material;

    void Start() { material = GetComponent<MeshRenderer>().material; }

    float CalculateDistance(Vector3 position) { return Vector3.Distance(Camera.main.transform.position, position); }
    void Update()
    {
        material.SetFloat("_Distance", Mathf.Clamp(CalculateDistance(transform.position) * multiplier - range, 0, material.GetFloat("_MaxDistance") - 1));
    }
}