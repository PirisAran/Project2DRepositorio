using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxBehaviour : MonoBehaviour
{
    AudioSource _audioSource;
    [Tooltip("0 empieza desde el principio y 1 desde el final, es para hacer que algunos audios suenen antes, si hace falta")]
    [Range(0f,1f)]
    [SerializeField] float _startTimeFraction = 0;
    [Range(0f, 1f)]
    [SerializeField] float _startFadeSoundFraction = 1;
    float _fadeStartTime;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.time = _audioSource.clip.length * _startTimeFraction;
        _fadeStartTime = _audioSource.clip.length * _startFadeSoundFraction;
        
        if (!_audioSource.loop)
        {
            StartCoroutine(DestroyAfterSoundPlays());
        }
    }

    private IEnumerator DestroyAfterSoundPlays()
    {
        StartCoroutine(DoFadeEffect());
        yield return new WaitForSeconds(_audioSource.clip.length + 1);
        Destroy(gameObject);
    }

    private IEnumerator DoFadeEffect()
    {
        yield return new WaitForSeconds(_fadeStartTime);
        float timeLeft = _audioSource.clip.length - _fadeStartTime;
        float timer = timeLeft;
        float oVolume = _audioSource.volume;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(oVolume, 0.0f, Mathf.Clamp01(timeLeft - timer / timeLeft));
        }
    }

    public void DestroySoundImmediatly()
    {
        Destroy(gameObject);
    }
}
