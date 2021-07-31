using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerSizeController : MonoBehaviour
{
    [SerializeField] private Transform _leftController;
    [SerializeField] private Transform _rightController;
    [SerializeField] private float _maxControllerSpread = 1.3f;
    
    private bool _leftHeld;
    private bool _rightHeld;
    private bool _resizing;
    private float _startingDistance;
    private Vector3 _resizeStartScale;
    private Vector3 _initialScale;
    [SerializeField] private XRRayInteractor[] _rays;
    [SerializeField] private LineRenderer[] _lineRenderers;
    [SerializeField] private float _maxScale;
    [SerializeField] private float _maxRayEndPointDistance = 1000f;
    [SerializeField] private float _maxRayWidth = 0.75f;

    private void Awake()
    {
        _initialScale = transform.localScale;
        UpdateRay();
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
            
            var scalePercent = (_startingDistance - distance) / _maxControllerSpread + 1f;
            transform.localScale = _resizeStartScale * scalePercent;

            UpdateRay();
        }
    }

    private void UpdateRay()
    {
        // Scale down the ray interactor and ray line renderer width
        var scaleFactor = transform.localScale.magnitude / _maxScale;
        foreach (var ray in _rays)
        {
            ray.endPointDistance = Mathf.Lerp(_maxRayEndPointDistance, 0f, scaleFactor);
            ray.controlPointDistance = Mathf.Lerp(_maxRayEndPointDistance / 2f, 0f, scaleFactor);
            ray.controlPointHeight = Mathf.Lerp(_maxRayEndPointDistance / 4f, 0f, scaleFactor);
        }

        foreach (var lineRenderer in _lineRenderers)
            lineRenderer.widthMultiplier = scaleFactor;
    }

    private void OnGUI()
    {
        GUILayout.Label($"Player local scale: {transform.localScale}");
        GUILayout.Label($"Player world scale: {transform.lossyScale}");
        GUILayout.Label($"Approximate player height (Unity units): {GetComponent<CapsuleCollider>().height * transform.lossyScale}");
    }
}
