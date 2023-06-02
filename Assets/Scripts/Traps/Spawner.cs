using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner: MonoBehaviour
{
    [SerializeField]
    GameObject _prefab;

    [SerializeField]
    Transform _spawnPoint;
    public GameObject SpawnOne()
    {
        GameObject obj = Instantiate(_prefab, _spawnPoint.position, Quaternion.identity);
        Debug.Log("Spawned: " + _prefab.name);
        return obj;
    }


}
