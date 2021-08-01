using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrinkrayBullet : MonoBehaviour
{
    private TrailRenderer trailRenderer;

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

            GameObject shrinkableObject = other.gameObject;

            shrinkableObject.transform.localScale = shrinkableObject.transform.localScale / 2;

            Debug.Log("shrinking done");
        }
    }
}
