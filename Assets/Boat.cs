using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Boat : MonoBehaviour
{
    [SerializeField] private Transform playerArea, exitBoatArea;

    public void PlayerEnter(XRRig player)
    {
        //Set player to appropriate scale
        player.transform.localScale = new Vector3(0.065f, 0.065f, 0.065f);

        //turn off the player's faux gravity while in boat
        player.GetComponent<FauxGravityBody>().enabled = false;

        //place player at playerArea
        player.transform.localPosition = playerArea.localPosition;
        player.transform.localRotation = playerArea.localRotation;
    }

    public void PlayerExit(XRRig player)
    {
        //Set player to appropriate scale
        player.transform.localScale = new Vector3(1f, 1f, 1f);

        //place player at playerArea
        player.transform.localPosition = exitBoatArea.localPosition;
        player.transform.localRotation = exitBoatArea.localRotation;

        //turn back on the player's faux gravity while in boat
        player.GetComponent<FauxGravityBody>().enabled = true;
    }

}
