using System;
using System.Collections;
using System.Linq;
using UI;
using UnityEngine;

public class Achievements : MonoBehaviour
{
	[Serializable]
	public class Achievement
	{
		public string ID;
		public string name;
		public string description;
		public bool awarded;
		public Sprite icon;
	}
	
	private static Achievements _instance;
	[SerializeField] private Achievement[] _achievements;
	[SerializeField] private AchievementNotification _achievementNotificationPrefab;
	[SerializeField] private Transform _achievementSpawnPoint;
	[SerializeField] private float _notificationDuration = 5f;


	public AudioSource audioSource;
	public AudioClip audioClip;

	private void Awake()
	{
		_instance = this;
		
		Load();
	}

	public static void Award(string achievementID)
	{
		// Check if the achievement exists
		var foundAchievement = _instance._achievements.FirstOrDefault(achievement => achievement.ID == achievementID);
		if (foundAchievement == null)
		{
			Debug.LogError($"Couldn't find an achievement with name {achievementID}");
			return;
		}
		
		// If the achievement hasn't already been unlocked
		if (!foundAchievement.awarded)
		{
			// Award the achievement
			_instance.Award(foundAchievement);
		}
	}

	private void Award(Achievement achievement)
	{
		Debug.Log($"Awarded achievement {achievement.ID}");
		achievement.awarded = true;
		
		// Show the notification
		StartCoroutine(ShowAchievementNotification(achievement));
		
		// Save
		Save();
	}

	private IEnumerator ShowAchievementNotification(Achievement achievement)
	{
		var notification = Instantiate(_achievementNotificationPrefab, _achievementSpawnPoint.position, Quaternion.identity, _achievementSpawnPoint);
		notification.Init(achievement);
		audioSource.PlayOneShot(audioClip);

		yield return new WaitForSeconds(_notificationDuration);
		
		Destroy(notification.gameObject);
	}

	private void Save()
	{
		Debug.Log($"Saving achievements");
	}

	public void Load()
	{
		Debug.Log($"Loading achievements");
	}
}