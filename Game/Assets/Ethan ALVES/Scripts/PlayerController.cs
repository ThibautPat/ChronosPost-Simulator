using NYX;
using System;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] float walk = 1.6f;
    [SerializeField] float gravity = 9.81f;
    [SerializeField] LayerMask groundMask;

    [Header("References")]
    [SerializeField] public new Transform renderer;
    [SerializeField] public new Transform camera;

    // Privates
    CharacterController character;
    InputManager inputs;
    Animator animator;
    float velocity;
    Vector3 slide;

    // States
    bool isCarrying;
    bool isGrounded;
    bool isSliding;
    bool isMoving;

    void Start()
    {
        character = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        inputs = InputManager.instance;
    }

    #region Utils

    Vector3 AdjustDirection(Vector3 velocity)
    {
        Vector3 origin = transform.position;
        float length = 0.2f;
        QueryTriggerInteraction query = QueryTriggerInteraction.Ignore;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, length, groundMask, query))
        {
            Quaternion slope = Quaternion.FromToRotation(Vector3.up, hit.normal);
            Vector3 adjusted = slope * velocity;

            if(adjusted.y < 0) { return adjusted; }
        }
        return velocity;
    }
    void AdjustSlide()
    {
        Vector3 origin = transform.position + new Vector3(0, character.height, 0);
        float length = character.height * 2;
        QueryTriggerInteraction query = QueryTriggerInteraction.Ignore;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, length, groundMask, query))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);

            if (angle >= character.slopeLimit)
            { slide = Vector3.ProjectOnPlane(new Vector3(0, velocity, 0), hit.normal); return; }
        }

        slide = Vector3.zero;
    }

    #endregion

    void Update()
    {
        // Update Inputs
        float horizontal = inputs.GetAxis("Horizontal");
        float vertical = inputs.GetAxis("Vertical");

        // Update Velocity
        isMoving = horizontal != 0 || vertical != 0;
        float input = isMoving ? 1 : 0;
        Vector3 direction = transform.forward * input;
        direction = AdjustDirection(direction);

        // Update Gravity
        isGrounded = character.isGrounded;
        if (!isGrounded) { velocity -= (gravity * Time.deltaTime * 4); }
        else { velocity = 0; } AdjustSlide();

        // Update Movement
        Vector3 movement = (direction * walk) + (new Vector3(0, velocity, 0));
        isSliding = slide != Vector3.zero;
        if (isSliding) { movement = slide; }
        character.Move(movement * Time.deltaTime);

        // Update Character Rotation
        if (isMoving)
        {
            float angle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg + renderer.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);
        }

        // Update Animations
        animator.SetBool("Walk", isMoving);
        animator.SetBool("Slide", isSliding);

        animator.SetLayerWeight(1, Mathf.SmoothStep(animator.GetLayerWeight(1), isCarrying ? 1 : 0, Time.deltaTime * 16));
    }

    public IEnumerator Hide(Action callback)
    {
        animator.SetTrigger("Hide");
        yield return new WaitForSeconds(0.5f);
        callback.Invoke();
    }
    public void Carry(bool value) { isCarrying = value; }
}