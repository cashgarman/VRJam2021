using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchRocket : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip audioClip;

    public bool launch = false;
    public float liftSpeed = 10;
    public GameObject bubbles;
    private Vector3 _initPosition;
    private float i = 0;

    private void Start()
    {
        _initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (launch)
        {
            GetComponent<ConstantForce>().relativeForce = Vector3.up * liftSpeed;
            bubbles.SetActive(true);
            if (i < 15) { i += Time.deltaTime; }
            else { Reset(); }  
        }
    }

    private void Reset()
    {
        launch = false;
        bubbles.SetActive(false);
        i = 0;
        GetComponent<ConstantForce>().relativeForce = new Vector3(0,0,0);
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        //GetComponent<ConstantForce>().relativeForce = new Vector3(0, 0, 0);
        
        transform.position = _initPosition;
    }

    public void LaunchIt()
    {
        launch = true;
        Achievements.Award("ShootForTheMoon");
        audioSource.PlayOneShot(audioClip);
    }
}
