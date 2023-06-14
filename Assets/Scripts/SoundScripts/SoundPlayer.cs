using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] List<GameObject> _soundsPrefabsList = new List<GameObject>();

    public GameObject PlaySound()
    {
        var sound = GetRandomSound();
        return SoundManager.InstantiateSound(sound, transform.position);
    }

    private GameObject GetRandomSound()
    {
        return _soundsPrefabsList[Random.Range(0, _soundsPrefabsList.Count)];
    }
}