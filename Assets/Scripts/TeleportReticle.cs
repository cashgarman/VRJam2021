using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportReticle : MonoBehaviour
{
	private Vector3 _initialScale;

	private void Awake()
	{
		_initialScale = transform.localScale;
	}

	private void OnEnable()
	{
		FindObjectOfType<PlayerSizeController>().AddReticle(this);
	}

	private void OnDisable()
	{
		FindObjectOfType<PlayerSizeController>().RemoveReticle(this);
	}

	private void OnDestroy()
	{
		FindObjectOfType<PlayerSizeController>().RemoveReticle(this);
	}

	public void SetScale(float scaleFactor)
	{
		transform.localScale = _initialScale * scaleFactor;
	}
}
