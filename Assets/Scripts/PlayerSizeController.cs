using System.Collections.Generic;
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
    [SerializeField] private float _rayWidth = 8f;
    [SerializeField] private float _maxReticleScale = 1f;

    private bool _leftHeld;
    private bool _rightHeld;
    private bool _resizing;
    private float _startingDistance;
    private Vector3 _resizeStartScale;
    private Vector3 _initialScale;
    private SphericalContinuousMoveProvider _continuousMovement;
    private float _scaleFactor;

    public List<TeleportReticle> Reticules { get; set; } = new List<TeleportReticle>();

    private void Awake()
    {
        _initialScale = transform.localScale;
        _continuousMovement = GetComponent<SphericalContinuousMoveProvider>();
        
        _scaleFactor = transform.localScale.magnitude / _maxScale;
        
        // Initialize the ray and movement scale 
        UpdateRay();
        UpdateMovementSpeed();
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
        }
        else if (_resizing && (!_leftHeld || !_rightHeld))
        {
            _resizing = false;
        }

        if (_resizing)
        {
            // Get the current local space distance between the controllers
            var distance = Vector3.Distance(_leftController.localPosition, _rightController.localPosition) / _maxControllerSpread;
            
            // MATH!
            var scalePercent = (_startingDistance - distance) / _maxControllerSpread + 1f;
            transform.localScale = Vector3.Max(_resizeStartScale * scalePercent, _minScale * Vector3.one);
            _scaleFactor = transform.localScale.magnitude / _maxScale;

            // Update the player's movement speed
            UpdateMovementSpeed();

            // Update the ray
            UpdateRay();
            
            // Update the reticules
            UpdateReticles();
        }
    }

    private void UpdateMovementSpeed()
    {
        _continuousMovement.MoveSpeed = Mathf.Lerp(0f, _continuousMovement.moveSpeed, _scaleFactor);
    }

    private void UpdateRay()
    {
        // Scale down the ray interactor and ray line renderer width
        foreach (var ray in _rays)
        {
            ray.endPointDistance = Mathf.Lerp(_maxRayEndPointDistance, 0f, _scaleFactor);
            ray.controlPointDistance = Mathf.Lerp(_maxRayEndPointDistance / 2f, 0f, _scaleFactor);
            ray.controlPointHeight = Mathf.Lerp(_maxRayEndPointDistance / 4f, 0f, _scaleFactor);
        }

        foreach (var lineVisual in _lineVisuals)
        {
            Debug.Log("scale factor: " + _scaleFactor);
            lineVisual.lineLength = Mathf.Lerp(0f, _maxRayEndPointDistance, _scaleFactor);
            lineVisual.widthCurve.keys[0].value = Mathf.Lerp(0f, _rayWidth, _scaleFactor);
            Debug.Log("key1: " + lineVisual.widthCurve.keys[0].value);
            lineVisual.widthCurve.keys[1].value = Mathf.Lerp(0f, _rayWidth, _scaleFactor);
            Debug.Log("key2: " + lineVisual.widthCurve.keys[1].value);
        }
    }

    private void UpdateReticles()
    {
        foreach (var reticle in Reticules)
        {
            reticle.SetScale(_scaleFactor);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label($"Player local scale: {transform.localScale}");
        GUILayout.Label($"Player world scale: {transform.lossyScale}");
        GUILayout.Label($"Approximate player height (Unity units): {GetComponent<CapsuleCollider>().height * transform.lossyScale}");
    }

    public void AddReticle(TeleportReticle reticle)
    {
        Reticules.Add(reticle);
        reticle.SetScale(_scaleFactor);
    }

    public void RemoveReticle(TeleportReticle reticle)
    {
        Reticules.Remove(reticle);
    }
}
