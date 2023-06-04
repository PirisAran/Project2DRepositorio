using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserBehaviour : MonoBehaviour
{
    [Header("Scripts Utilizados")]
    [SerializeField] FireDamager _fireDamager;

    [Header("Valores FSM")]
    [SerializeField] float _idleTime;
    [SerializeField] float _chargingTime;
    [SerializeField] float _activeTime;
    [SerializeField] float _chargingWarningsNum = 3;
    [SerializeField] float _delay;
    private bool _firstIdle = true;
    
    [Space]
    [SerializeField] float _idleParticleSpeed = 0;
    [SerializeField] float _chargingParticleSpeed = 1;
    [SerializeField] float _activeParticleSpeed = 6;

    [Space]
    [SerializeField] AnimationClip _colliderUpAnim;
    [SerializeField] AnimationClip _colliderDownAnim;

    private Animation _animation;
    private ParticleSystem _particleSystem;
    private ParticleSystem.EmissionModule _particleEmission;
    private ParticleSystem.MainModule _mainModule;

    private enum States { Idle, Charging, Active}
    private States _currentState;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _particleSystem = GetComponent<ParticleSystem>();
        _particleEmission = _particleSystem.emission;
        _mainModule = _particleSystem.main;

        _activeTime += _colliderDownAnim.length + _colliderUpAnim.length;
    }

    void Start()
    {
        ChangeState(States.Idle);
        SetParticlesSpeed(_idleParticleSpeed);
    }

    // Update is called once per frame
    private IEnumerator DoActiveState()
    {
        SetParticlesSpeed(_activeParticleSpeed);
        _fireDamager.SetCanDamage(true);
        _animation.Play(_colliderUpAnim.name);
        Debug.Log("up anim");
        yield return new WaitForSeconds(_activeTime - _colliderDownAnim.length);
        StartCoroutine(SetActiveParticles(false));
        _animation.Play(_colliderDownAnim.name);
        yield return new WaitForSeconds(_colliderDownAnim.length);
        ChangeState(States.Idle);
    }

    private IEnumerator DoChargingState()
    {
        yield return new WaitForSeconds(_chargingTime);
        ChangeState(States.Active);
    }

    private IEnumerator DoIdleState()
    {
        if (_firstIdle)
        {
            _firstIdle = false;
            yield return new WaitForSeconds(_delay);
        }
        SetParticlesSpeed(_idleParticleSpeed);
        _fireDamager.SetCanDamage(false);
        yield return new WaitForSeconds(_idleTime);
        ChangeState(States.Charging);
    }

    private void ChangeState(States nextState)
    {
        _currentState = nextState;
        Debug.Log("changed to " + _currentState);

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

    private IEnumerator SetActiveParticles(bool v)
    {
        float upAnimTime = _colliderUpAnim.length;
        float downAnimTime = _colliderDownAnim.length;

        float timer = v ? upAnimTime : downAnimTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            if (v)
                _mainModule.startSpeed = Mathf.Lerp(_idleParticleSpeed, _activeParticleSpeed, Mathf.Clamp01((upAnimTime - timer) / upAnimTime));
            else
                _mainModule.startSpeed = Mathf.Lerp(_activeParticleSpeed, _idleParticleSpeed, Mathf.Clamp01((upAnimTime - timer) / upAnimTime));
            yield return null;
        }
    }

    private void SetParticlesSpeed(float speed)
    {
        _mainModule.startSpeed = speed;
    }
}
