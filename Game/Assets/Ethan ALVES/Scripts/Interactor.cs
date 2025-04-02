using UnityEngine;
using UnityEngine.Events;

// Declare interface to make the script procedural
interface IInteractable
{
    /// <summary>
    /// Called each frame when the player can interact with the object
    /// </summary>
    /// <param name="interactor"></param>
    void OnInteractable(Interactor interactor);

    /// <summary>
    /// Called once when the player starts interacting with the object
    /// </summary>
    /// <param name="interactor"></param>
    void OnInteractBegin(Interactor interactor);

    /// <summary>
    /// Called each frame when the player is interacting with the object
    /// </summary>
    /// <param name="interactor"></param>
    void OnInteract(Interactor interactor);

    /// <summary>
    /// Called once when the player stops interacting with the object
    /// </summary>
    /// <param name="interactor"></param>
    void OnInteractEnd(Interactor interactor);
}

public class Interactor : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("The key used by player to interact.")]
    [SerializeField] KeyCode interactKey = KeyCode.E;

    [Range(0f, 8f)]
    [Tooltip("The maximum distance on which the player can interact.")]
    [SerializeField] float maxDistance = 2.6f;

    [Tooltip("The layer(s) that will be used to interact.")]
    [SerializeField] LayerMask interactMask;

    [Tooltip("Can the interaction system work with 'trigger' colliders ? (not recommended to change).")]
    [SerializeField] QueryTriggerInteraction interactWithTriggers = QueryTriggerInteraction.Ignore;

    [Space]

    [Header("Events")]
    [Tooltip("This event executes when the player can interact with an object.")]
    [SerializeField] UnityEvent OnInteractable;

    [Tooltip("This event executes when the player starts to interact with an object")]
    [SerializeField] UnityEvent OnInteractBegin;

    [Tooltip("This event executes when the player is interacting with an object.")]
    [SerializeField] UnityEvent OnInteract;

    [Tooltip("This event executes when the player stops to interact with an object.")]
    [SerializeField] UnityEvent OnInteractEnd;

    [Header("References")]
    [SerializeField] public Transform placeholder;

    // Shared Vars
    [HideInInspector] public GameObject player;
    [HideInInspector] public PlayerController controller;

    [HideInInspector] public bool interactable, interacting;

    // Privates
    RaycastHit hit;
    IInteractable interfaceComponent;

    void Start() { player = transform.parent.gameObject; }

    void Update()
    {
        // Perform a RaycastAll to detect multiple hits along the ray
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, interactMask, interactWithTriggers);

        // Iterate through hits to find an interactable object
        interactable = false; // Reset interactable state
        foreach (RaycastHit raycastHit in hits)
        {
            GameObject hitObject = raycastHit.collider.gameObject;

            // Check if the hit object belongs to the interactable layer
            if (interactMask.Contains(hitObject.layer))
            {
                interactable = true;
                hit = raycastHit; // Update the active hit to this object
                hitObject.TryGetComponent<IInteractable>(out interfaceComponent);
                break; // Exit the loop after finding the first interactable object
            }
        }

        #region Handle Interaction
        if (interactable)
        {
            // Check for user inputs
            if (Input.GetKeyDown(interactKey))
            {
                // Execute abstract method using the interface
                interfaceComponent?.OnInteractBegin(this);

                // Execute unity-event
                OnInteractBegin.Invoke();

                // Update state
                interacting = true;
            }

            // Execute abstract method using the interface
            interfaceComponent?.OnInteractable(this);

            // Execute unity-event
            OnInteractable.Invoke();
        }

        if (interacting)
        {
            // Execute abstract method using the interface
            interfaceComponent?.OnInteract(this);

            // Execute unity-event
            OnInteract.Invoke();
        }

        if (Input.GetKeyUp(interactKey))
        {
            // Execute abstract method using the interface
            interfaceComponent?.OnInteractEnd(this);

            // Execute unity-event
            OnInteractEnd.Invoke();

            // Update state
            interacting = false;
        }
        #endregion
    }

    void OnDrawGizmos()
    {
        Vector3 direction;

        // Change displayed data depending on editor/game mode ( to avoid errors in editor )
        if (Application.isPlaying && interactable)
        {
            Gizmos.color = Color.red;
            direction = hit.point;
        }
        else
        {
            Gizmos.color = Color.green;
            direction = transform.position + transform.forward * maxDistance;
        }

        // Display gizmos
        Gizmos.DrawLine(transform.position, direction);
        Gizmos.DrawWireSphere(direction, 0.1f);
    }
}