using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Shrinkray : MonoBehaviour
{
    public GameObject rayVisual;

    public Transform raySpawnPoint;

    public float raySpeed = 10.0f;

    public void ShootRay()
    {

        // Spawn an instance of the ray prefab
        GameObject spawnedRay = Instantiate(rayVisual, raySpawnPoint.position, raySpawnPoint.rotation);

        // Set the speed and direction of the bullet by referencing it's Rigidbody component
        spawnedRay.GetComponent<Rigidbody>().velocity = raySpeed * raySpawnPoint.forward;

        //Destroy the ray after some seconds so there aren't a bunch lying around
        Destroy(spawnedRay, 5f);
    }
}
