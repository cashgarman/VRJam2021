using UnityEngine;

public class Shrinkray : MonoBehaviour
{
    public GameObject rayVisual;

    public Transform raySpawnPoint;

    public float raySpeed = 10.0f;
    private AudioSource _audioSource;
    
    [SerializeField] private AudioClip _shootingSound;

    public int successfulShrinks, successfulGrows;

    [SerializeField] private int shrinksForAchievement = 3;
    [SerializeField] private int growsForAchievement = 1;

    public void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ShootRay()
    {
        // Spawn an instance of the ray prefab
        GameObject spawnedRay = Instantiate(rayVisual, raySpawnPoint.position, raySpawnPoint.rotation);

        // Set the speed and direction of the bullet by referencing it's Rigidbody component
        spawnedRay.GetComponent<Rigidbody>().velocity = raySpeed * raySpawnPoint.forward;

        if (spawnedRay.GetComponent<ShrinkrayBullet>())
        {
            spawnedRay.GetComponent<ShrinkrayBullet>().rayGun = this;
        } else if (spawnedRay.GetComponent<GrowrayBullet>())
        {
            spawnedRay.GetComponent<GrowrayBullet>().rayGun = this;
        }

        //Destroy the ray after some seconds so there aren't a bunch lying around
        Destroy(spawnedRay, 5f);
        
        // Play the shooting sound
        _audioSource.PlayOneShot(_shootingSound);
    }

    public void SuccessfulShot()
    {
        Debug.Log("successfulshot! S/Gs: " + successfulShrinks + ", " + successfulGrows);
        if(successfulShrinks == shrinksForAchievement)
        {
            Achievements.Award("ShrinkingMachine");
        } else if (successfulGrows == growsForAchievement)
        {
            Achievements.Award("LargeWorldAfterAll");
        }
    }
}
