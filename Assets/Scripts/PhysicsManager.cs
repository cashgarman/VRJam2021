using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    public static PhysicsManager Instance { get; private set; }
    
    [SerializeField] private PhysicMaterial _defaultPhysicsMaterial;
    [SerializeField] private float _defaultDrag = 0.1f;
    [SerializeField] private float _gravity = -9.8f;

    public static float DefaultDrag => Instance._defaultDrag;
    public static PhysicMaterial DefaultPhysicsMaterial => Instance._defaultPhysicsMaterial;
    public static float Gravity => Instance._gravity;

    void Awake()
    {
        Instance = this;
        
        // For all the rigid bodies in the scene
        var rigidBodies = FindObjectsOfType<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            // If the rigid body uses gravity
            if (rigidBody.useGravity)
            {
                // Turn off the gravity
                rigidBody.useGravity = false;
                
                // Add a gravity attractor to the object if it doesn't already have one
                if (rigidBody.GetComponent<FauxGravityBody>() == null)
                {
                    rigidBody.gameObject.AddComponent<FauxGravityBody>().freezeRotation = false;
                }
                
                // Add the default drag to the rigid body
                rigidBody.drag = _defaultDrag;
                
                // If the object doesn't already have a physics material
                var collider = rigidBody.GetComponent<Collider>();
                if (collider.material == null)
                    // Use the default physics material
                    collider.material = _defaultPhysicsMaterial;
            }
        }
    }
}
