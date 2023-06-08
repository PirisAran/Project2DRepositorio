using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] List<GameObject> _soundsPrefabsList = new List<GameObject>();

    public GameObject PlaySound()
    {
        var sound = GetRandomSound();
        SoundManager.InstantiateSound(sound);
        return SoundManager.InstantiateSound(sound);
    }

    private GameObject GetRandomSound()
    {
        return _soundsPrefabsList[Random.Range(0, _soundsPrefabsList.Count)];
    }
}