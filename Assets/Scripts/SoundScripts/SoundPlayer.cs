using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] List<GameObject> _soundsPrefabsList = new List<GameObject>();

    public GameObject PlaySound()
    {
        var sound = GetRandomSound();
        Debug.Log((sound == null) + transform.parent.transform.parent.name);
        return SoundManager.InstantiateSound(sound);
    }

    private GameObject GetRandomSound()
    {
        var soundChosen = _soundsPrefabsList[Random.Range(0, _soundsPrefabsList.Count)];
        return soundChosen;
    }
}