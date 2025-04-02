using NYX;
using UnityEngine;
using Cinemachine;
using System.Collections;

public class VehicleInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] VehicleController vehicle;

    // Privates
    PlayerController player;
    InputManager inputs;
    bool inside = false;

    void Start()
    {
        inside = false;
        inputs = InputManager.instance;
    }

    public void OnInteractBegin(Interactor interactor) { StartCoroutine(EnterVehicle(interactor)); }
    void Update()
    {
        if(inputs.GetKeyDown("Interact"))
        {
            if (inside && player != null) { StartCoroutine(ExitVehicle()); }
        }
    }

    IEnumerator EnterVehicle(Interactor interactor)
    {
        player = interactor.player.GetComponent<PlayerController>();

        player.camera.GetComponent<CinemachineFreeLook>().Priority = 0;
        StartCoroutine(player.Hide(() => {
            player.transform.parent = transform;
            player.gameObject.SetActive(false);

            vehicle.parking = false;
            vehicle.controlled = true;
            vehicle.camera.GetComponent<CinemachineVirtualCamera>().Priority = 1;

            inside = true;
        }));

        yield return null;
    }
    IEnumerator ExitVehicle()
    {
        vehicle.parking = true;
        vehicle.controlled = false;
        vehicle.camera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        player.camera.GetComponent<CinemachineFreeLook>().Priority = 1;

        yield return new WaitForSeconds(1.65f);

        player.gameObject.SetActive(true);
        player.transform.parent = null;
        player = null;
        inside = false;
    }

    #region Unused Interface Methods

    public void OnInteract(Interactor interactor) { }
    public void OnInteractEnd(Interactor interactor) { }
    public void OnInteractable(Interactor interactor) { }

    #endregion
}