using System;
using System.Linq;
using UnityEngine;

public class Achievements : MonoBehaviour
{
	[Serializable]
	private class Achievement
	{
		public string name;
		public string description;
		public bool awarded;
	}
	
	private static Achievements _instance;
	[SerializeField] private Achievement[] _achievements;

	private void Awake()
	{
		_instance = this;
		
		Load();
	}

	public static void Award(string achievementName)
	{
		// Check if the achievement exists
		var foundAchievement = _instance._achievements.FirstOrDefault(achievement => achievement.name == achievementName);
		if (foundAchievement == null)
		{
			Debug.LogError($"Couldn't find an achievement with name {achievementName}");
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
		Debug.Log($"Awarded achievement {achievement.name}");
		achievement.awarded = true;
		
		// Save
		Save();
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