using UnityEngine;
using UnityEngine.UI;

public class UIWaypoint : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Transform target;

    // Private variables
    Text distance;
    Canvas parentCanvas;
    RectTransform waypoint;

    void Start()
    {
        distance = GetComponentInChildren<Text>();
        waypoint = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        if(target == null) { return; }

        Rect adjustedRect = RectTransformUtility.PixelAdjustRect(waypoint, parentCanvas);

        float minX = adjustedRect.width / 2 + 50;
        float maxX = Screen.width - minX;

        float minY = adjustedRect.height / 2 + 50;
        float maxY = Screen.height - minY;

        Vector2 position = Camera.main.WorldToScreenPoint(target.position);

        if (Vector3.Dot((target.position - Camera.main.transform.position), Camera.main.transform.forward) < 0)
        {
            if (position.x < Screen.width / 2) position.x = maxX;
            else position.x = minX;
        }

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        transform.position = position;

        float scale = CalculateScale(position, minX, maxX, minY, maxY);
        waypoint.localScale = Vector3.one * scale;

        distance.text = CalculateDistance(target.position).ToString("F000");
    }

    float CalculateDistance(Vector3 position) { return Vector3.Distance(Camera.main.transform.position, position); }
    float CalculateScale(Vector2 position, float minX, float maxX, float minY, float maxY)
    {
        float horizontalDistance = Mathf.Min(position.x - minX, maxX - position.x);
        float verticalDistance = Mathf.Min(position.y - minY, maxY - position.y);
        float distanceToEdge = Mathf.Min(horizontalDistance, verticalDistance);

        float normalizedDistance = Mathf.Clamp01(distanceToEdge / 50);

        return Mathf.Lerp(0, 1, normalizedDistance);
    }
}