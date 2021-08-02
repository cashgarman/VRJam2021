using UnityEngine;

public class Shrinkray : MonoBehaviour
{
    public GameObject rayVisual;

    public Transform raySpawnPoint;

    public float raySpeed = 10.0f;
    private AudioSource _audioSource;
    
    [SerializeField] private AudioClip _shootingSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ShootRay()
    {
        // Spawn an instance of the ray prefab
        GameObject spawnedRay = Instantiate(rayVisual, raySpawnPoint.position, raySpawnPoint.rotation);

        // Set the speed and direction of the bullet by referencing it's Rigidbody component
        spawnedRay.GetComponent<Rigidbody>().velocity = raySpeed * raySpawnPoint.forward;

        //Destroy the ray after some seconds so there aren't a bunch lying around
        Destroy(spawnedRay, 5f);
        
        // Play the shooting sound
        _audioSource.PlayOneShot(_shootingSound);
    }
}
