using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner: MonoBehaviour
{
    //Molaria ferho amb un type, y una factory amb reflection, amb DamageFireTypes, etc, de moment aixi sta be,
    
    [SerializeField]
    GameObject WaterDropPrefab;

    [SerializeField]
    Transform SpawnPoint;

    [SerializeField]
    float SpawnRate = 1;
    float _lastTimeSpawned;

    private void Update()
    {
        if (Time.time - _lastTimeSpawned >= SpawnRate)
            SpawnWaterDrop();
    }

    private void SpawnWaterDrop()
    {
        var waterDrop = Instantiate(WaterDropPrefab, SpawnPoint.position, Quaternion.identity);
        waterDrop.GetComponent<WaterDrop>().Init();
        _lastTimeSpawned = Time.time;
    }
}
