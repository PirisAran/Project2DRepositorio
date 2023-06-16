using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBats : MonoBehaviour
{
    PlayerDetector _playerDetector;

    [SerializeField] GameObject _particleBatsPrefab;
    ParticleSystem _particleSystem;
    ParticleSystem.EmissionModule _particleSystemEmission;

    [SerializeField] SoundPlayer _batsSound;
    bool _firstTimeEntered = true;

    private void Awake()
    {
        _playerDetector = GetComponent<PlayerDetector>();
        _particleSystem = _particleBatsPrefab.GetComponent<ParticleSystem>();
        _particleSystemEmission = _particleSystem.emission;
        _particleSystemEmission.enabled = false;
    }
    private void OnEnable()
    {
        _playerDetector.OnPlayerDetected += EmitBats;
    }

    private void OnDisable()
    {
        _playerDetector.OnPlayerDetected -= EmitBats;
    }

    private void EmitBats()
    {
        if (_firstTimeEntered)
        {
            _particleSystem.Emit(13);
            _batsSound.PlaySound();
            _firstTimeEntered = false;
        }
    }

}
