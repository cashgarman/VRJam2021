using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomARang : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;

    private bool flying = false;
    public float flightTime = 1.5f;
    public Transform startPos;
    public float returnSpeed = 15;
   

    // Update is called once per frame
    void Update()
    {
        if (flying)
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * 1000f);
            if (flightTime <= 0)
            {
                Vector3.MoveTowards(transform.position, startPos.position, 15);
            }
            flightTime -= Time.deltaTime;
        }
    }

    public void Thrown()
    {
        Achievements.Award("AussieThrowDown");
        flying = true;
        startPos = transform;
        audioSource.PlayOneShot(audioClip);
    }

    public void Grabbed()
    {
        flightTime = 1.5f;
        flying = false;
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Ground"))
        {
            flying = false;
        }
    }
}
