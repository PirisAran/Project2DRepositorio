using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserBehaviour : MonoBehaviour
{
    [Header("Scripts Utilizados")]
    [SerializeField] FireDamager _fireDamager;
    [SerializeField] SoundPlayer _startSound;
    [SerializeField] SoundPlayer _waterRunningSound;

    [Header("Valores FSM")]
    [SerializeField] float _idleTime;
    [SerializeField] float _chargingTime;
    [SerializeField] float _activeTime;
    [SerializeField] float _delay;
    private bool _firstIdle = true;
    
    [Space]
    [SerializeField] AnimationClip _colliderUpAnim;
    [SerializeField] AnimationClip _colliderDownAnim;
    [SerializeField] AnimationClip _colliderChargingAnim;

    [SerializeField] Animation _animation;
    private ParticleSystem _particleSystem;
    private ParticleSystem.EmissionModule _particleEmission;
    private ParticleSystem.MainModule _mainModule;
    private ParticleSystem.VelocityOverLifetimeModule _velocityOverLifeTimeModule;
    private ParticleSystem.TriggerModule _triggerModule;

    private enum States { Idle, Charging, Active}
    private States _currentState;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleEmission = _particleSystem.emission;
        _mainModule = _particleSystem.main;
        _velocityOverLifeTimeModule = _particleSystem.velocityOverLifetime;
        _triggerModule = _particleSystem.trigger;
        _activeTime += _colliderDownAnim.length + _colliderUpAnim.length;
    }

    void Start()
    {
        _mainModule.gravityModifier = transform.parent.localScale.y;
        ChangeState(States.Idle);
        ActivateParticles(false);
    }

    // Update is called once per frame
    private IEnumerator DoActiveState()
    {

        _animation.Play(_colliderUpAnim.name);
        Debug.Log("up anim");

        SetParticlesSpeed(1);

        var startSound = _startSound.PlaySound();
        var runningSound = _waterRunningSound.PlaySound();
        yield return new WaitForSeconds(_activeTime - _colliderDownAnim.length);
        _animation.Play(_colliderDownAnim.name);
        Debug.Log("down anim");

        StartCoroutine(SetActiveParticlesOverTime(false));

        runningSound.GetComponent<SfxBehaviour>().DestroyAfterSecondsWithFade(_colliderDownAnim.length);
        yield return new WaitForSeconds(_colliderDownAnim.length);
        ChangeState(States.Idle);
    }

    private IEnumerator DoChargingState()
    {
        var sound = _waterRunningSound.PlaySound();
        SetParticlesSpeed(.5f);
        ActivateParticles(true);
        _fireDamager.SetCanDamage(true);
        _animation.Play(_colliderChargingAnim.name);
        yield return new WaitForSeconds(_chargingTime/2);
        sound.GetComponent<SfxBehaviour>().DestroyAfterSecondsWithFade(_chargingTime / 2);
        yield return new WaitForSeconds(_chargingTime/2);
        ChangeState(States.Active);
    }

    private IEnumerator DoIdleState()
    {
        if (_firstIdle)
        {
            _firstIdle = false;
            yield return new WaitForSeconds(_delay);
        }
        ActivateParticles(false);
        _fireDamager.SetCanDamage(false);
        yield return new WaitForSeconds(_idleTime);
        ChangeState(States.Charging);
    }

    private void ChangeState(States nextState)
    {
        _currentState = nextState;

        switch (_currentState)
        {
            case States.Idle:
                StartCoroutine(DoIdleState());
                break;
            case States.Charging:
                StartCoroutine(DoChargingState());
                break;
            case States.Active:
                StartCoroutine(DoActiveState());
                break;
        }
    }


    private void ActivateParticles(bool v)
    {
        _particleEmission.enabled = v;
    }

    private IEnumerator SetActiveParticlesOverTime(bool v)
    {
        float upAnimTime = _colliderUpAnim.length;
        float downAnimTime = _colliderDownAnim.length;

        float timer = v ? upAnimTime : downAnimTime;

        timer *= 0.5f;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            if (v)
                _velocityOverLifeTimeModule.speedModifier = Mathf.Clamp01((upAnimTime - timer) / upAnimTime);
            else
                _velocityOverLifeTimeModule.speedModifier = 1 - Mathf.Clamp01((upAnimTime - timer) / downAnimTime);
            yield return null;
        }
    }

    private void SetParticlesSpeed(float speed)
    {
        _velocityOverLifeTimeModule.speedModifier = speed;
    }
}
