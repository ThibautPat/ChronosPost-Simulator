using NYX;
using UnityEngine;

public class Lights : MonoBehaviour
{
    [Header("Setup")]
    [Range(0, 16)] [SerializeField] float smoothing = 8;
    [Range(0, 1)] [SerializeField] float intensity = 0.025f;

    [Header("References")]
    [SerializeField] Light[] lights;

    // Privates
    InputManager inputs;

    void Start() { inputs = InputManager.instance; }

    void Update()
    {
        float input = inputs.GetAxis("Vertical") < 0 ? Mathf.Abs(inputs.GetAxis("Vertical")) : 0;

        foreach (Light light in lights)
        { light.intensity = Mathf.SmoothStep(light.intensity, input * intensity, Time.deltaTime * smoothing); }
    }
}