﻿using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : SoundPlayer
{
    [SerializeField] List<GameObject> _soundsPrefabs = new List<GameObject>();

    public override void PlaySound()
    {
        var sound = GetRandomSound();
        SoundManager.InstantiateSound(sound);
    }

    private GameObject GetRandomSound()
    {
        return _soundsPrefabs[Random.Range(0, _soundsPrefabs.Count)];
    }
}