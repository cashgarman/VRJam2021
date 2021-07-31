using UnityEngine;

public class FauxGravityBody : MonoBehaviour
{
    public bool freezeRotation = true;
    
    private FauxGravityAttractor attractor;

    private void Awake()
    {
        if (attractor == null)
            attractor = FindObjectOfType<FauxGravityAttractor>();

        var rigidBody = GetComponent<Rigidbody>();
        
        // If the object doesn't already have a physics material
        var collider = rigidBody.GetComponent<Collider>();
        if (collider.material == null)
            // Use the default physics material
            collider.material = PhysicsManager.DefaultPhysicsMaterial;
        
        rigidBody.useGravity = false;
        rigidBody.drag = PhysicsManager.DefaultDrag;
    }

    private void Start()
    {
        if(freezeRotation)
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        attractor.Attract(transform);
    }
}
