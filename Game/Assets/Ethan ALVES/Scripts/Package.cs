using UnityEngine;

public class Package : MonoBehaviour
{
    [Header("Setup")]
    [Range(0, 100)] [SerializeField] public float health = 100;
    [SerializeField] public float resistance = 2.5f;
    [SerializeField] public bool useBlendshapes = true;

    [Header("References")]
    [SerializeField] public SkinnedMeshRenderer[] meshes;
    [SerializeField] public int blendshapeIndex = 0;

    // Privates
    new Rigidbody rigidbody;

    void Start()
    {
        TryGetComponent(out rigidbody);
    }

    void Update() { }

    private void OnCollisionEnter(Collision collision)
    {
        if(rigidbody.velocity.magnitude > resistance)
        {
            health -= rigidbody.velocity.magnitude / resistance;
            if(useBlendshapes) { meshes[0].SetBlendShapeWeight(blendshapeIndex, 100 - (health)); }
            if (health <= 0) { Destroy(gameObject); }
        }
    }
}