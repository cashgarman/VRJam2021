using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BoatButton : MonoBehaviour
{
    [Header("Button Animation Properties")]
    [SerializeField]
    private GameObject buttonMeshObject;
    [SerializeField]
    private float downTravelSpeed = .1f;
    //[SerializeField]
    //private float upTravelSpeed = 0.01f;
    [SerializeField]
    private float buttonTravelDistance = 50f;
    //private float currentDestination = 0;
    private Vector3 startPos;

    //Target for the buttons action
    public GameObject target;
    private bool buttonDown;

    public AudioSource audioSource;
    public AudioClip audioClip;


    // Use this for initialization
    void Start()
    {
        if (buttonMeshObject != null)
        {
            startPos = buttonMeshObject.transform.localPosition;
        }

        //gameObject.tag = "button3D"; //This is required if the tag based solution is selected. Can be disabled once it has been set, it's simply just to ensure that the user (you), is aware of this tag's importance.

    }

    private void OnTriggerEnter(Collider other)
    {
        //Animate the button
        ButtonDown();
        audioSource.PlayOneShot(audioClip);

        //TODO: Put player in the boat at scale
        target.GetComponent<Boat>().PlayerEnter(other.gameObject.GetComponentInParent<XRRig>());
    }

    void ButtonDown()
    {
        float travelled = 0;
        buttonMeshObject.transform.localPosition = startPos;
        while (travelled < buttonTravelDistance)
        {
            travelled += downTravelSpeed;
            buttonMeshObject.transform.localPosition -= new Vector3(0, 1, 0) * downTravelSpeed;
        }
        buttonMeshObject.transform.localPosition = startPos;
    }
}
