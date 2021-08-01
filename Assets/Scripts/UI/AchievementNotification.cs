using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class AchievementNotification : MonoBehaviour
	{
		[SerializeField] private TMP_Text _nameText;
		[SerializeField] private TMP_Text _descriptionText;
		[SerializeField] private Image _icon;

		public void Init(Achievements.Achievement achievement)
		{
			_nameText.text = achievement.name;
			_descriptionText.text = achievement.description;
			_icon.sprite = achievement.icon;
		}

		private void Update()
		{
			transform.forward = Camera.main.transform.forward;
		}
	}
}