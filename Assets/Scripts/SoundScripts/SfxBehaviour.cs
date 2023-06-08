using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxBehaviour : MonoBehaviour
{
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (!_audioSource.loop)
        {
            StartCoroutine(DestroyAfterSoundPlays());
        }
    }

    private IEnumerator DestroyAfterSoundPlays()
    {
        yield return new WaitForSeconds(_audioSource.clip.length);
        Destroy(gameObject);
    }

    public void DestroySoundImmediatly()
    {
        Destroy(gameObject);
    }
}
