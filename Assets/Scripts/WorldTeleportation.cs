using System.Reflection;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WorldTeleportation : BaseTeleportationInteractable
{
	public void Teleport(XRRayInteractor xrRayInteractor)
	{
		Debug.Log($"Teleport reached with {xrRayInteractor}");
		
		// HACK: SendTeleportRequest SHOULD be protected, preview... whatcha gonna do? :|
		var sendTeleportRequestMethod = GetType().GetMethod("SendTeleportRequest", BindingFlags.Instance | BindingFlags.NonPublic);
		sendTeleportRequestMethod.Invoke(this, new[]{xrRayInteractor});
	}
}
