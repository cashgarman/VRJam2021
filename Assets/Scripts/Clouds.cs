using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Clouds : MonoBehaviour
{
    [SerializeField] private int _numLowLevelClouds = 20;
    [SerializeField] private GameObject _lowLevelCloudPrefab;
    [SerializeField] private Transform _lowLevelCloudContainer;
    [SerializeField] private float _lowLevelCloudHeight = 10f;
    
    private readonly List<GameObject> _clouds = new List<GameObject>();

    void Start()
    {
        SpawnClouds();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SpawnClouds();
    }

    private void SpawnClouds()
    {
        foreach(var existingCloud in _clouds)
            Destroy(existingCloud.gameObject);
        
        // Add the low level clouds
        for (var i = 0; i < _numLowLevelClouds; ++i)
        {
            var cloud = Instantiate(_lowLevelCloudPrefab, _lowLevelCloudContainer);
            cloud.transform.position = Random.onUnitSphere * _lowLevelCloudHeight;
            cloud.transform.up = (cloud.transform.position - transform.position).normalized;
            _clouds.Add(cloud);
        }
    }
}
