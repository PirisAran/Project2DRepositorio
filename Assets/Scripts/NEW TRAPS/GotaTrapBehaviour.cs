using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotaTrapBehaviour : MonoBehaviour
{
    [Header("Scripts Utilizados")]
    [SerializeField] Spawner _spawner;

    [Space]
    [Header("Valores de Spawn Rate")]
    [SerializeField] float _spawnRate = 1;
    [SerializeField] Transform _spawnLocation;
    float _lastTimeSpawned;

    private void FixedUpdate()
    {
        if (CanSpawn())
        {
            _spawner.SpawnOne(_spawnLocation.transform.position, Quaternion.identity);
            _lastTimeSpawned = Time.time;
        }
    }

    private bool CanSpawn()
    {
        return Time.time - _lastTimeSpawned >= _spawnRate;
    }
}
