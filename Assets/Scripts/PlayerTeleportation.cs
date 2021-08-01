using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerTeleportation : MonoBehaviour
{
	public XRController _leftController;
	public XRController _rightController;
	
	public InputHelpers.Button teleportRayShowButton;

	public float activationThreshold = 0.1f;

	private void Update()
	{
		// Show and hide the teleport 'hands' based on the controller inputs
		//var left = InputHelpers.IsPressed(_leftController.inputDevice, teleportRayShowButton,out ///_, .1f);
		//
		//_leftController.gameObject.SetActive(left);
		//_rightController.gameObject.SetActive(_rightController.inputDevice.IsPressed//(teleportRayShowButton, out _, .1f));

		if (_leftController)
		{
			_leftController.gameObject.SetActive(CheckIfActivated(_leftController));
		}
  
        if (_rightController)
        {
			_rightController.gameObject.SetActive(CheckIfActivated(_rightController));
		}
	}

	public bool CheckIfActivated(XRController controller)
	{
		InputHelpers.IsPressed(controller.inputDevice, teleportRayShowButton, out bool isActivated, activationThreshold);
		return isActivated;
	}
}		