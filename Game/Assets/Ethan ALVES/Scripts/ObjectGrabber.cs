using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjectGrabber : MonoBehaviour, IInteractable
{
    [Header("Setup")]
    [Range(0, 4)] [SerializeField] float smoothing = 2;

    // Privates
    new Rigidbody rigidbody;
    new Collider collider;
    bool grabbed = false;

    void Start()
    {
        grabbed = false;
        TryGetComponent(out rigidbody);
        transform.GetChild(0).TryGetComponent(out collider);
    }

    void Update()
    {
        if (grabbed)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, Vector3.zero, Time.deltaTime * (16 * smoothing));
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * (8 * smoothing));
        }
    }

    public void OnInteractBegin(Interactor interactor)
    {
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        collider.enabled = false;
        transform.parent = interactor.placeholder.transform;
        grabbed = true;
        interactor.player.GetComponent<PlayerController>().Carry(true);
    }

    public void OnInteractEnd(Interactor interactor)
    {
        if (grabbed)
        {
            transform.parent = null;
            collider.enabled = true;
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
            rigidbody.AddForce((interactor.transform.forward + Vector3.up) * rigidbody.mass * 4, ForceMode.Impulse);
            grabbed = false;
            interactor.player.GetComponent<PlayerController>().Carry(false);
        }
    }

    #region Unused Interface Methods

    public void OnInteract(Interactor interactor) { }
    public void OnInteractable(Interactor interactor) { }

    #endregion
}