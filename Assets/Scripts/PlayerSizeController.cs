using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSizeController : MonoBehaviour
{
    private bool _leftHeld;
    private bool _rightHeld;
    [SerializeField] private Transform _leftController;
    [SerializeField] private Transform _rightController;
    private bool _resizing;
    private float _startingDistance;
    private float _maxDistance;

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
            _startingDistance = Vector3.Distance(_leftController.position, _rightController.position);
            Debug.Log($"Started resizing with controller distance: {_startingDistance}");
        }
        else if (_resizing && (!_leftHeld || !_rightHeld))
        {
            _resizing = false;
            Debug.Log("Stopped resizing");
        }

        if (_resizing)
        {
            var distance = Vector3.Distance(_leftController.position, _rightController.position);
            _maxDistance = Mathf.Max(_maxDistance, distance);
            
            Debug.Log($"Max distance: {_maxDistance} ({distance})");

            var percentStretched = distance / _maxDistance;
            Debug.Log($"percentStretched: {percentStretched}");
        }
    }
}
