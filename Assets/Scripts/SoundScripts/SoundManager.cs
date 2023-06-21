using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager _soundManager;

    private void Awake()
    {
        _soundManager = this;
    }

    public static GameObject InstantiateSound(GameObject sound, Vector3 position)
    {
        if (_soundManager==null)
        {
            return null;
        }
        return _soundManager._InstantiateSound(sound, position);
    }

    private GameObject _InstantiateSound(GameObject sound, Vector3 position)
    {
        var s = Instantiate(sound, position, Quaternion.identity);
        s.transform.parent = transform;
        return s;
    }
}
