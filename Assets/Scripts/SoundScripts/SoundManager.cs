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

    public static GameObject InstantiateSound(GameObject sound)
    {
        return _soundManager._InstantiateSound(sound);
    }

    private GameObject _InstantiateSound(GameObject sound)
    {
        return Instantiate(sound, transform);
    }
}
