using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public enum StemType
{
	Synth,
	Strings,
	Keys,
	Bass,
}

public class MusicManager : MonoBehaviour
{
	[Serializable]
	public class Stem
	{
		public StemType _type;
		public AudioClip _clip;
		public float _maxVolume = 1f;
		[HideInInspector] public AudioSource _source;
	}

	public static MusicManager instance;

	public Stem[] _stems;
	[SerializeField] private float _fadeDuration = 3f;

	private void Awake()
	{
		if (instance != null)
			throw new Exception("There are more than 1 MusicManagers in the scene");

		instance = this;
	}

	private void Start()
	{
		// For each stem in the track
		foreach (var stem in _stems)
		{
			// Create an audio source on this object for the stem
			var source = stem._source = gameObject.AddComponent<AudioSource>();
			source.clip = stem._clip;
			
			// Start the volume at zero
			source.volume = 0f;
			
			// Play the stem
			source.loop = true;
			source.Play();
		}
	}

	public void StartStem(string type)
	{
		// Find the stem source
		var stem = _stems.FirstOrDefault(stem => stem._type.ToString() == type);
		if (stem != null)
		{
			// Fade the stem in
			stem._source.DOFade(stem._maxVolume, _fadeDuration);
		}
	}
	
	public void StopStem(string type)
	{
		// Find the stem source
		var stem = _stems.FirstOrDefault(stem => stem._type.ToString() == type);
		if (stem != null)
		{
			// Fade the stem out
			stem._source.DOFade(0f, _fadeDuration);
		}
	}

	/*private void OnGUI()
	{
		foreach (var stem in _stems)
		{
			stem._source.volume = GUILayout.Toggle(stem._source.volume != 0, stem._clip.name) ? stem._maxVolume : 0f;
			GUILayout.Label($"{stem._clip.name}: {stem._source.time:F1}s");
			// stem._source.volume = EditorGUILayout.Slider(stem._clip.name, stem._source.volume, 0f, 1f);
		}
	}*/

	public static void FadeOut(float duration)
	{
		foreach (var stem in instance._stems)
		{
			stem._source.DOFade(0, duration);
		}
	}
}







