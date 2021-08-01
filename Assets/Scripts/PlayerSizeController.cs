using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerSizeController : MonoBehaviour
{
    [SerializeField] private Transform _leftController;
    [SerializeField] private Transform _rightController;
    [SerializeField] private float _maxControllerSpread = 1.3f;
    [SerializeField] private XRRayInteractor[] _rays;
    [SerializeField] private XRInteractorLineVisual[] _lineVisuals;
    [SerializeField] private float _maxScale;
    [SerializeField] private float _maxRayEndPointDistance = 1000f;
    [SerializeField] private float _maxRayWidth = 0.75f;
    [SerializeField] private float _minScale = 0.0001f;
    
    private bool _leftHeld;
    private bool _rightHeld;
    private bool _resizing;
    private float _startingDistance;
    private Vector3 _resizeStartScale;
    private Vector3 _initialScale;
    private SphericalContinuousMoveProvider _continuousMovement;
    [SerializeField] private float _rayWidth = 8f;

    private void Awake()
    {
        _initialScale = transform.localScale;
        _continuousMovement = GetComponent<SphericalContinuousMoveProvider>();
        UpdateRay(transform.localScale.magnitude / _maxScale);
        UpdateMovementSpeed(transform.localScale.magnitude / _maxScale);
    }

    public void OnLeftResizeActivated(InputAction.CallbackContext context)
    {
        if (context.started)
            _leftHeld = true;
        else if (context.canceled)
            _leftHeld = false;
    }
    
    public void OnRightResizeActivated(InputAction.CallbackContext context)
    {
        if (context.started)
            _rightHeld = true;
        else if (context.canceled)
            _rightHeld = false;
    }
    
    void Update()
    {
        if (!_resizing && _leftHeld && _rightHeld)
        {
            _resizing = true;
            _startingDistance = Vector3.Distance(_leftController.localPosition, _rightController.localPosition) / _maxControllerSpread;
            _resizeStartScale = transform.localScale;
            Debug.Log($"Started resizing with controller distance: {_startingDistance}");
        }
        else if (_resizing && (!_leftHeld || !_rightHeld))
        {
            _resizing = false;
            Debug.Log("Stopped resizing");
        }

        if (_resizing)
        {
            // Get the current local space distance between the controllers
            var distance = Vector3.Distance(_leftController.localPosition, _rightController.localPosition) / _maxControllerSpread;
            
            // MATH!
            var scalePercent = (_startingDistance - distance) / _maxControllerSpread + 1f;
            transform.localScale = Vector3.Max(_resizeStartScale * scalePercent, _minScale * Vector3.one);
            var scaleFactor = transform.localScale.magnitude / _maxScale;

            // Update the player's movement speed
            UpdateMovementSpeed(scaleFactor);

            // Update the ray
            UpdateRay(scaleFactor);
        }
    }

    private void UpdateMovementSpeed(float scaleFactor)
    {
        _continuousMovement.MoveSpeed = Mathf.Lerp(0f, _continuousMovement.moveSpeed, scaleFactor);
    }

    private void UpdateRay(float scaleFactor)
    {
        // Scale down the ray interactor and ray line renderer width
        foreach (var ray in _rays)
        {
            ray.endPointDistance = Mathf.Lerp(_maxRayEndPointDistance, 0f, scaleFactor);
            ray.controlPointDistance = Mathf.Lerp(_maxRayEndPointDistance / 2f, 0f, scaleFactor);
            ray.controlPointHeight = Mathf.Lerp(_maxRayEndPointDistance / 4f, 0f, scaleFactor);
        }

        foreach (var lineVisual in _lineVisuals)
        {
            lineVisual.lineLength = Mathf.Lerp(0f, _maxRayEndPointDistance, scaleFactor);
            lineVisual.widthCurve.keys[0].value = Mathf.Lerp(0f, _rayWidth, scaleFactor);
            lineVisual.widthCurve.keys[1].value = Mathf.Lerp(0f, _rayWidth, scaleFactor);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label($"Player local scale: {transform.localScale}");
        GUILayout.Label($"Player world scale: {transform.lossyScale}");
        GUILayout.Label($"Approximate player height (Unity units): {GetComponent<CapsuleCollider>().height * transform.lossyScale}");
    }
}
