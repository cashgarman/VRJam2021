using UnityEngine;

public class ClearChildrenOnAwake : MonoBehaviour
{
	private void Awake()
	{
		foreach (Transform child in transform)
			Destroy(child.gameObject);
	}
}
