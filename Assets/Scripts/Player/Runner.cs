using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{

    //Components
    Rigidbody2D _rb;
    Thrower _thrower;

    //Movement
    float _horizontalMov;
    [SerializeField]
    float NoFireSpeed = 8, FireSpeed = 5;
    float _currentStateSpeed;

    [SerializeField] SoundPlayer _stepsSounds;
    [SerializeField] float _timeBetweenSteps;
    [SerializeField] GameObject _particleStepsPrefab;
    ParticleSystem _particleSystem;
    ParticleSystem.EmissionModule _particleSystemEmission;

    bool _isRunning;

    public float CurrentSpeed { get { return _currentStateSpeed; } set { _currentStateSpeed = value; } }

    public Vector2 Forward => new Vector2(_horizontalMov, 0).normalized;
    public float XSpeed { get; set; }

    private void Awake()
    {
        _particleSystem = _particleStepsPrefab.GetComponent<ParticleSystem>();
        _particleSystemEmission = _particleSystem.emission;
        _rb = GetComponent<Rigidbody2D>();
        _currentStateSpeed = FireSpeed;
        _thrower = GetComponent<Thrower>();
        StartCoroutine(StepsSound());
    }

    private void Update()
    {
        MoveInput();
        UpdateMove();
        UpdateParticleSteps();
    }

    private void MoveInput()
    {
        _horizontalMov = Input.GetAxisRaw("Horizontal");
    }
    private void UpdateMove()
    {
        Move();
    }
    private void Move()
    {
        var vel = new Vector2(_horizontalMov * _currentStateSpeed, _rb.velocity.y);
        _rb.velocity = vel;
        XSpeed = vel.x;

        _isRunning = vel.x != 0 && vel.y == 0;
    }

    public void ChangeSpeed()
    {
        _currentStateSpeed = _thrower.HasFire ? FireSpeed : NoFireSpeed;
    }

    IEnumerator StepsSound()
    {
        while (true)
        {
            if (_isRunning)
            {
                _timeBetweenSteps = 0.4f;
                _stepsSounds.PlaySound();
                if (_currentStateSpeed == NoFireSpeed)
                {
                    _timeBetweenSteps = 0.27f;
                }
            }
            yield return new WaitForSeconds(_timeBetweenSteps);
        }
    }

    private void UpdateParticleSteps()
    {
        _particleSystemEmission.enabled = _rb.velocity.y == 0;
    }
}
   
    
