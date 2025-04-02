using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineScript : MonoBehaviour
{
    Collider collider;
    public float force;
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        collider = GetComponent<Collider>();
    }

    void DisaperMine()
    {
        GameObject child = this.transform.GetChild(0).gameObject;
        child.SetActive(false);
    }

    IEnumerator FUCKSound()
    {
        source.Play();
        collider.enabled = false;

        DisaperMine();

        while (source.isPlaying)
        {
            yield return null;
        }
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wheel"))
        {
            Debug.Log("Boom");
            Rigidbody Onsenbranle = other.gameObject.GetComponent<Rigidbody>();
            Onsenbranle.AddForce(Vector3.up * force * Onsenbranle.mass, ForceMode.Impulse);
            StartCoroutine(FUCKSound());
        }
    }
}
