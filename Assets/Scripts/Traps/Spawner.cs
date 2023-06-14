using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner: MonoBehaviour
{
    [SerializeField]
    GameObject _prefab;

    public GameObject SpawnOne(Vector2 pos, Quaternion rot)
    {
        GameObject obj = Instantiate(_prefab, pos, rot);
        return obj;
    }


}
