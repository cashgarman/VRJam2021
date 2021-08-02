using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkrayBullet : MonoBehaviour
{
    private TrailRenderer trailRenderer;

    public Shrinkray rayGun;
    [SerializeField] private AudioClip shrinkSound;

    private void Awake()
    {
        trailRenderer = this.GetComponent<TrailRenderer>();
    }

    private void OnCollisionEnter(Collision other)
    {
        trailRenderer.enabled = false;
        Destroy(this);

        Debug.Log("shrinkray hit " + other.collider.name);

        if (other.gameObject.CompareTag("Shrinkable"))
        {
            Debug.Log(other.collider.name + " is shrinkable");

            rayGun.GetComponent<AudioSource>().PlayOneShot(shrinkSound);

            GameObject shrinkableObject = other.gameObject;

            shrinkableObject.transform.localScale = shrinkableObject.transform.localScale / 2;

            Debug.Log("shrinking done");

            rayGun.successfulShrinks++;
            rayGun.SuccessfulShot();
        }
    }
}
