using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] float height = 10;

    [Header("References")]
    [SerializeField] Transform mapRenderer;
    [SerializeField] Transform mainRenderer;
    [SerializeField] RectTransform arrow;

    Transform player;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        mapRenderer.position = new Vector3(player.position.x, height, player.position.z);
        mapRenderer.eulerAngles = new Vector3(90, mainRenderer.eulerAngles.y, 0);
        arrow.localEulerAngles = new Vector3(0, 0, mainRenderer.eulerAngles.y - player.eulerAngles.y);
    }
}