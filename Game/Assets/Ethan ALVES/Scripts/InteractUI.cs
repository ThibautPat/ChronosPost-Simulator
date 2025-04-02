using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] GameObject interactUI;

    // Privates
    bool shown = false;

    public void Show() { shown = true; }
    void Update() { interactUI.SetActive(shown); shown = false; }
}