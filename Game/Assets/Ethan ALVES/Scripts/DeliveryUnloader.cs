using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeliveryUnloader : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] string packageTag = "Package";
    [Range(0, 8)] [SerializeField] int expectedPackages = 1;

    [Space]

    [SerializeField] UnityEvent OnCompleted;
    [SerializeField] UnityEvent OnUnloaded;

    [Header("References")]
    [SerializeField] Text text;

    // Shared
    [HideInInspector] public bool completed = false;

    // Privates
    int current = 0;

    void Update()
    {
        completed = current >= expectedPackages;
        if (completed) { OnCompleted.Invoke(); }
        text.text = $"{current}/{expectedPackages}";
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(packageTag))
        {
            if(!completed)
            {
                current++;
                OnUnloaded.Invoke();
                Destroy(other.gameObject);
            }
        }
    }
}