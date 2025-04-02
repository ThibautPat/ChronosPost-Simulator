using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] CursorLockMode cursor = CursorLockMode.Locked;
    [Range(1, 360)] [SerializeField] int framerate = 60;

    void Start() { Cursor.lockState = cursor; }
    void Update() { Application.targetFrameRate = framerate; }
    public void ShowCursor() { Cursor.lockState = CursorLockMode.None; }
    public void HideCursor() { Cursor.lockState = CursorLockMode.Locked; }
}