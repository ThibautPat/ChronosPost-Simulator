using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    public int NumberOfWantedBox = 0;
    public int ActualNumberOfBox = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PackageBox"))
        {
            Destroy(other.gameObject);
            ActualNumberOfBox += 1;
            Debug.Log("Package is in the delivery zone!");

            CheckCompletion();
        }
    }

    void CheckCompletion()
    {
        if (ActualNumberOfBox >= NumberOfWantedBox)
        {
            Debug.Log("Delivery zone is complete! Destroying the zone.");
            Destroy(this.gameObject);
        }
    }
}
