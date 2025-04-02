using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] Animator door;
    [SerializeField] Animator panel;

    // Privates
    bool isOpen = false;

    void Start() { isOpen = false; }

    void Update()
    {
        door.SetBool("Open", isOpen);
        panel.SetBool("Open", isOpen);
    }

    public void OnInteractBegin(Interactor interactor) { isOpen = !isOpen; }

    #region Unused Interface Methods

    public void OnInteract(Interactor interactor) { }
    public void OnInteractEnd(Interactor interactor) { }
    public void OnInteractable(Interactor interactor) { }

    #endregion
}