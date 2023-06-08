using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager _soundManager;

    //Dictionary<string, GameObject> _allSoundsPrefabs = new Dictionary<string, GameObject>();
    //Dictionary<string, GameObject> _trapsSoundPrefabs = new Dictionary<string, GameObject>();
    //Dictionary<string, GameObject> _playerSoundPrefabs = new Dictionary<string, GameObject>();
    //Dictionary<string, GameObject> _fireSoundPrefabs = new Dictionary<string, GameObject>();
    //Dictionary<string, GameObject> _musicSoundPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        _soundManager = this;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
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
