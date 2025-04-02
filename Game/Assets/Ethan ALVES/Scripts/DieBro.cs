using UnityEngine;
using UnityEngine.Events;

public class DieBro : MonoBehaviour
{
    [SerializeField] public UnityEvent lol;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Wheel"))
        {
            lol.Invoke();
        }
    }
}