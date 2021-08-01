using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Sounds
{
    None,
    GlowstickCrack,
    HeavyGlowstickImpact,
    MediumGlowstickImpact,
    LightGlowstickImpact,
    HeavyPebbleImpact,
    MediumPebbleImpact,
    LightPebbleImpact,
    GlowstickGrab,
    PebbleGrab,
}

public class SoundManager : MonoBehaviour
{
    [Serializable]
    private class Sound
    {
        public Sounds soundType;
        public AudioClip[] clips;

        public override string ToString()
        {
            return $"{soundType}: {clips.Length} sounds";
        }
    }

    private static SoundManager instance;

    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("There are 2 SoundManagers in the scene, BAD.");
            return;
        }

        instance = this;
    }

    public static void Play(Transform source, Sounds soundType)
    {
        // Bail if no sound to be played
        if (soundType == Sounds.None)
            return;
        
        // Find a matching sound in the list of sounds
        //var matchingSound = instance.sounds.FirstOrDefault(sound => sound.soundType == soundType);    // USING LINQ
        Sound matchingSound = null;
        foreach(var sound in instance.sounds)
        {
            if (sound.soundType == soundType)
            {
                matchingSound = sound;
                break;
            }
        }

        if(matchingSound == null)
        {
            Debug.LogError($"Couldn't find a sound with type {soundType}");
            return;
        }

        // Grab any existing audio source on the source object
        var audioSource = source.GetComponent<AudioSource>();

        // If no audio source exists on the source object
        if(audioSource == null)
        {
            // Add one
            audioSource = source.gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.spatialize = true;
            audioSource.spatialBlend = 1f;
        }

        // Play the random sound clip from the available clips
        if (matchingSound.clips.Length > 0)
        {
            var clip = matchingSound.clips[Random.Range(0, matchingSound.clips.Length)];
            audioSource.PlayOneShot(clip);

            Debug.Log($"Playing sound {clip.name} on {source.name}");
        }
    }
}