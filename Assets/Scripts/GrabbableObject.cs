using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class GrabbableObject : MonoBehaviour
{

	public PlayerSizeController sizeController;
	private XRGrabInteractable grabInteractable;
	public Vector3 startingScale;

	private void Awake()
	{
		this.gameObject.layer = LayerMask.NameToLayer("Grabbable");
		sizeController = FindObjectOfType<PlayerSizeController>();
		grabInteractable = this.GetComponent<XRGrabInteractable>();

		grabInteractable.selectEntered.AddListener(OnPickedUp);
		grabInteractable.selectExited.AddListener(OnDropped);
	}

    private void OnDropped(SelectExitEventArgs arg0)
    {
		Debug.Log("Grabbable dropped");
		sizeController.RemoveHeldObject(this);	
    }

    private void OnPickedUp(SelectEnterEventArgs arg0)
    {
		Debug.Log("Grabbable picked up");
		sizeController.AddHeldObject(this);
		startingScale = transform.localScale;
		Debug.Log(startingScale);
	}




}
