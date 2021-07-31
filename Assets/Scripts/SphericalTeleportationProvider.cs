using UnityEngine.XR.Interaction.Toolkit;

public class SphericalTeleportationProvider : TeleportationProvider
{
    protected override void Update()
    {
        if (!validRequest || !BeginLocomotion())
            return;

        var xrRig = system.xrRig;
        if (xrRig != null)
        {
            // NOTE: We don't need this in our use case because our gravitation system adjust our rotation based on where we're at on the planet
            // switch (currentRequest.matchOrientation)
            // {
            //     case MatchOrientation.WorldSpaceUp:
            //         xrRig.MatchRigUp(Vector3.up);
            //         break;
            //     case MatchOrientation.TargetUp:
            //         xrRig.MatchRigUp(currentRequest.destinationRotation * Vector3.up);
            //         break;
            //     case MatchOrientation.TargetUpAndForward:
            //         xrRig.MatchRigUpCameraForward(currentRequest.destinationRotation * Vector3.up, currentRequest.destinationRotation * Vector3.forward);
            //         break;
            //     case MatchOrientation.None:
            //         // Change nothing. Maintain current rig rotation.
            //         break;
            //     default:
            //         Assert.IsTrue(false, $"Unhandled {nameof(MatchOrientation)}={currentRequest.matchOrientation}.");
            //         break;
            // }

            var heightAdjustment = xrRig.rig.transform.up * xrRig.cameraInRigSpaceHeight;

            var cameraDestination = currentRequest.destinationPosition + heightAdjustment;

            xrRig.MoveCameraToWorldLocation(cameraDestination);
        }

        EndLocomotion();
        validRequest = false;
    }
}
