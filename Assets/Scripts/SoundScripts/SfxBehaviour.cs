using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxBehaviour : MonoBehaviour
{
    AudioSource _audioSource;
    [Tooltip("0 empieza desde el principio y 1 desde el final, es para hacer que algunos audios suenen antes, si hace falta")]
    [Range(0f,1f)]
    [SerializeField] float _startTimeFraction;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.time = _audioSource.clip.length * _startTimeFraction;

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
