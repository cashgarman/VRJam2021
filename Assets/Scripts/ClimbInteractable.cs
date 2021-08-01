using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClimbInteractable : MonoBehaviour
{
    public void Grabbed()
    {
        
    } 

    public void Released()
    {
        Debug.Log("Released");
    }
}
