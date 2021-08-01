using System;
using UnityEngine;

public class TeleportReticle : MonoBehaviour
{
	private Vector3 _initialScale;

	private void Awake()
	{
		_initialScale = transform.localScale;
	}

	private void OnEnable()
	{
		Debug.Log($"Adding reticle {name}");
		FindObjectOfType<PlayerSizeController>().AddReticle(this);
	}

	private void OnDisable()
	{
		Debug.Log($"Disabling reticle {name}");
		FindObjectOfType<PlayerSizeController>().RemoveReticle(this);
	}

	private void OnDestroy()
	{
		Debug.Log($"Destroying reticle {name}");
		FindObjectOfType<PlayerSizeController>().RemoveReticle(this);
	}

	public void SetScale(float scaleFactor)
	{
		transform.localScale = _initialScale * scaleFactor;
	}
}
