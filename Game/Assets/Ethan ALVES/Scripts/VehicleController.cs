using NYX;
using System;
using UnityEngine;

[Serializable]
public class Wheel
{
    public Transform mesh;
    public WheelCollider collider;

    public bool canDrive = false;
    public bool canSteer = false;

    public void Update()
    {
        collider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        mesh.position = position; mesh.rotation = rotation;
    }

    public void Drive(float torque) { if (canDrive) { collider.motorTorque = torque; } }
    public void Brake(float brake) { collider.brakeTorque = brake; }
    public void Steer(float angle) { if (canSteer) { collider.steerAngle = angle; } }
}

public class VehicleController : MonoBehaviour
{
    [Header("Setup")]
    [Range(0, 90)] [SerializeField] float maximumSteering = 35f;
    [SerializeField] AnimationCurve steeringDistribution = AnimationCurve.EaseInOut(0, 1, 1, 0);

    [Range(0, 512)] [SerializeField] float maximumPower = 256f;
    [SerializeField] AnimationCurve powerDistribution = AnimationCurve.Linear(0, 1, 1, 0);

    [Space]

    [SerializeField][Range(0, 130)] float maximumSpeed = 70f;

    [Header("References")]
    [SerializeField] AudioClip startSound;
    [SerializeField] AudioClip engineSound;
    [SerializeField] Wheel[] wheels;
    [SerializeField] public new Transform camera;

    // Shared
    [HideInInspector] public bool parking = true;
    [HideInInspector] public bool controlled = false;
    bool toggle0 = false, toggle1 = false;

    // Privates
    InputManager inputs;
    AudioSource source;
    Rigidbody body;
    float speed;

    void Start()
    {
        parking = true;
        controlled = false;
        inputs = InputManager.instance;
        body = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Update Speed
        speed = body.velocity.magnitude * 3.6f;

        // Inputs
        float horizontal = inputs.GetAxis("Horizontal");
        float vertical = inputs.GetAxis("Vertical");

        foreach (Wheel wheel in wheels)
        {
            wheel.Update();
            if (controlled)
            {
                if (!toggle0) { source.PlayOneShot(startSound); toggle0 = true; }
                if (!source.isPlaying && !toggle1) { source.clip = engineSound; source.Play(); toggle1 = true; }
                source.pitch = 1f + (speed / maximumSpeed) *2;
                wheel.Steer(horizontal * maximumSteering * steeringDistribution.Evaluate(speed / maximumSpeed));
                wheel.Drive(vertical * maximumPower * powerDistribution.Evaluate(speed / maximumSpeed) * 4);
            }
            else { toggle0 = false; toggle1 = false; source.Stop(); }

            wheel.Brake(parking ? body.mass : 0);
        }
    }
}