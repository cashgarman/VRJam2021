using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowrayBullet : MonoBehaviour
{
    private TrailRenderer trailRenderer;

    public Shrinkray rayGun;

    private void Awake()
    {
        trailRenderer = this.GetComponent<TrailRenderer>();
    }

    private void OnCollisionEnter(Collision other)
    {
        trailRenderer.enabled = false;
        Destroy(this);

        Debug.Log("Growray hit " + other.collider.name);

        if (other.gameObject.CompareTag("Shrinkable"))
        {
            Debug.Log(other.collider.name + " is growable");

            GameObject shrinkableObject = other.gameObject;

            shrinkableObject.transform.localScale = shrinkableObject.transform.localScale * 2;

            Debug.Log("growing done");

            rayGun.successfulGrows++;
            rayGun.SuccessfulShot();
        }
    }
}
