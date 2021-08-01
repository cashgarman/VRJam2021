using UnityEngine;

public class Clouds : MonoBehaviour
{
    [SerializeField] private int _numLowLevelClouds = 20;
    [SerializeField] private GameObject _lowLevelCloudPrefab;
    [SerializeField] private Transform _lowLevelCloudContainer;
    [SerializeField] private float _lowLevelCloudHeight = 10f;

    void Start()
    {
        // Add the low level clouds
        for (var i = 0; i < _numLowLevelClouds; ++i)
        {
            var cloud = Instantiate(_lowLevelCloudPrefab, _lowLevelCloudContainer);
            cloud.transform.position = Random.onUnitSphere * _lowLevelCloudHeight;
        }
    }
}
