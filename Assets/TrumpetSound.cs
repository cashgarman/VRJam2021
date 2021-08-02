using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetSound : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip smallTrumpet;
    public AudioClip medTrumpet;
    public AudioClip largeTrumpet;

    public float smallThreshold;
    public float largeThreshold;

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.transform.localScale.magnitude <= smallThreshold)
        {
            audioSource.PlayOneShot(smallTrumpet);
        } else if (this.gameObject.transform.localScale.magnitude > smallThreshold && this.gameObject.transform.localScale.magnitude < largeThreshold)
        {
            audioSource.PlayOneShot(medTrumpet);
        } else if (this.gameObject.transform.localScale.magnitude >= largeThreshold)
        {
            audioSource.PlayOneShot(largeTrumpet);
        }
    }
}
