using UnityEngine;

public class CloudLayerRotation : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    
    private Vector3 _rotationAxis;

    private void Start()
    {
        _rotationAxis = Random.onUnitSphere;
    }

    void Update()
    {
        transform.Rotate(_rotationAxis, _speed * Time.deltaTime);
    }
}
