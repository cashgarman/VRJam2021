using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartController : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip audioClip;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("DartBoard"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            Achievements.Award("BullsEye");
            audioSource.PlayOneShot(audioClip);
        }
    }
}
