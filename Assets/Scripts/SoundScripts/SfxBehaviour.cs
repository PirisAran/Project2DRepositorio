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
        _audioSource.Play();
        var clipLenght = _audioSource.clip.length;
        _audioSource.time = clipLenght * _startTimeFraction;
        _fadeStartTime = (_audioSource.clip.length - _audioSource.time) * _startFadeSoundFraction;

        if (!_audioSource.loop)
        {
            DestroyAfterSoundPlays();
        }
    }
    private void DestroyAfterSoundPlays()
    {
        StartCoroutine(FadeEffectFromStartTime(_fadeStartTime));
    }

    private IEnumerator FadeEffectFromStartTime(float fadeStartTime)
    {
        yield return new WaitForSeconds(fadeStartTime);
        float oVolume = _audioSource.volume;
        float fadeDuration = _audioSource.clip.length - _audioSource.time;
        float timeElapsed = 0;
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(oVolume, 0.0f, Mathf.Clamp01(timeElapsed / fadeDuration));
            yield return null;
        }
        _audioSource.volume = 0;
        Destroy(gameObject);
    }

    public void DestroySoundImmediatly()
    {
        Destroy(gameObject);
    }

    public void DestroyAfterSecondsWithFade(float time) 
    {
        StartCoroutine(FadeEffect(time));
    }

    private IEnumerator FadeEffect(float fadeDuration)
    {
        float oVolume = _audioSource.volume;
        float timeElapsed = 0;
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(oVolume, 0.0f, Mathf.Clamp01(timeElapsed / fadeDuration));
            yield return null;
        }
        _audioSource.volume = 0;
        Destroy(gameObject);
    }

}
