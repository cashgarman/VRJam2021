using UnityEngine;

public class FauxGravityBody : MonoBehaviour
{
    public bool freezeRotation = true;
    
    public FauxGravityAttractor Attractor { get; private set; }

    private void Start()
    {
        Attractor = FindObjectOfType<FauxGravityAttractor>();

        var rigidBody = GetComponent<Rigidbody>();
        
        // If the object doesn't already have a physics material
        var collider = rigidBody.GetComponent<Collider>();
        if (collider.material == null)
            // Use the default physics material
            collider.material = PhysicsManager.DefaultPhysicsMaterial;
        
        rigidBody.useGravity = false;
        rigidBody.drag = PhysicsManager.DefaultDrag;
        
        if(freezeRotation)
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        Attractor.Attract(transform, freezeRotation);
    }
}
