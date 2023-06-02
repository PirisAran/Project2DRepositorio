using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;
using TecnocampusProjectII;

public class FireController : MonoBehaviour, IRestartLevelElement
{
                                             /* ---------- FIRE CONTROLLER ----------- */
    [SerializeField] Thrower _playerThrower;
    [SerializeField] Transform _attachedLocation;
    
    //Components    
    [SerializeField] List<SpriteRenderer> _sprites;
    Rigidbody2D _rb;
    FireGroundChecker _collCheck;

    [Header ("Light Parameters")]
    [SerializeField]
    Light2D _light;

    internal void BeThrown()
    {
        throw new NotImplementedException();
    }

    [SerializeField]
    float _maxLightRange = 6;
    [SerializeField]
    Color _lightColor;
    [SerializeField]
    GameObject _fireParticlesPrefab;
    public float LightRange => _lightRange;
    float _lightRange;

    internal void SubscribeToLvl(LevelController levelController)
    {
        levelController.AddRestartLevelElement(this);
    }

    [Space] 

    [Header("Light Effect Parameters")]
    [SerializeField] float _tremblingValue = 0.3f;
    [SerializeField] float _intervalTimeMax = 0.15f, _intervalTimeMin = 0.05f;
    float _lastTimeTremble;
    [SerializeField] float _lightExplosionRangeAdded = 5;
    [SerializeField] float _timeOfExpanding = 0.3f, _timeOfHolding = 0.3f, _timeOfDecreasing = 0.6f;
    float _explosionTimer;
    bool _isExploding = false;

    ParticleSystem _particleSystem;
    [SerializeField] float _minParticleSize = 0.3f, _maxParticleSize = 1.2f;
    [SerializeField] int _minParticleRate = 10, _maxParticleRate = 40;



    //Pick up and throw parameters--------------------
    [SerializeField]
    CircleCollider2D _pickUpCollider;
    [SerializeField]
    float _pickUpRadius = 1;
    public bool OnPickUpRange => _playerThrower.PickUpCollider.IsTouching(_pickUpCollider);
    
    //Health Parameters---------------------
    [SerializeField]
    float _maxFireHealth = 10;
    float _currentFireHealth;
    public float CurrentFireHealth { get { return _currentFireHealth;} set { _currentFireHealth = value;} }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _lightRange);
    }

    private void OnEnable()
    {
        _collCheck.OnLanding += OnLanding;
        CheckPoint.OnCheckPointActivated += OnCheckPointActivated;
    }

    private void OnDisable()
    {
        _collCheck.OnLanding -= OnLanding;
        CheckPoint.OnCheckPointActivated -= OnCheckPointActivated;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collCheck = GetComponent<FireGroundChecker>();
        _particleSystem = _fireParticlesPrefab.GetComponentInChildren<ParticleSystem>();
    }
    private void Start()
    {
        SetDefaultValues();
    }

    public void SetDefaultValues()
    {
        BePickedUp();
        _pickUpCollider.radius = _pickUpRadius;
        _lightRange = _maxLightRange;
        _currentFireHealth = _maxFireHealth;
        _light.color = _lightColor;
    }

    private void AdjustLightEffect()
    {
        var healthFraction = Mathf.Clamp01(_currentFireHealth / _maxFireHealth);
        _lightRange = Mathf.Lerp(0, _maxLightRange, healthFraction);

        //cambiar emision particulas
        var emissionRate = Mathf.Lerp(_minParticleRate, _maxParticleRate, healthFraction);
        var emission = _particleSystem.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(emissionRate);

        //cambiar tamaño particulas
        var maxSize = Mathf.Lerp(_minParticleSize, _maxParticleSize, healthFraction);
        var mainModule = _particleSystem.main;
        mainModule.startSize = new ParticleSystem.MinMaxCurve(maxSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAttached())
        {
            transform.position = _attachedLocation.position;
        }

        if (_isExploding)
        {
            UpdateExplosionEffect();
            return;
        }

        AdjustLightEffect();
        UpdateTremblingEffect();
    }


    /* ------ PICK UP AND BE THROWN ------ */
    public void BeThrown(Vector2 dir, float currentThrowSpeed)
    {
        SetAttached(false);
        _rb.velocity = dir * currentThrowSpeed;
        Show();

    }

    public void BePickedUp()
    {
        SetAttached(true);
        transform.localPosition = Vector2.zero;
    }

    private void SetAttached(bool v)
    {
        Debug.Log("attahced");
        _playerThrower.SetAttachFireToBody(v);
        _rb.bodyType = v ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = !v;
    }

    private bool IsAttached()
    {
        return _playerThrower.HasFire;
    }

    private void OnLanding()
    {
        _rb.velocity = Vector2.zero;
    }

    /* ----- ----- APPEARENCE (SHOW, HIDE, ETC) --------- */
    private void UpdateTremblingEffect()
    {
        if (Time.time - _lastTimeTremble >= Random.Range(_intervalTimeMin, _intervalTimeMax))
        {
            var tempLightRange = LightRange + Random.Range(-_tremblingValue, _tremblingValue);
            _light.pointLightOuterRadius = tempLightRange;
            _lastTimeTremble = Time.time;

            if (_light.pointLightOuterRadius <= 0)
            {
                _light.pointLightOuterRadius = 0.3f;
            }
        }

    }
    private void Hide()
    {
        _fireParticlesPrefab.SetActive(false);
        foreach (var sprite in _sprites)
        {
            sprite.enabled = false;
        }
    }
    void Show()
    {
        _fireParticlesPrefab.SetActive(true);
        foreach (var sprite in _sprites)
        {
            sprite.enabled = true;
        }
    }
    private void UpdateExplosionEffect()
    {
        if (_explosionTimer >= _timeOfDecreasing + _timeOfExpanding + _timeOfHolding)
        {
            _isExploding = false;
            _tremblingValue -= 0.5f;
            return;
        }

        float _increasingTimer = _explosionTimer;
        float _holdingTimer = _increasingTimer - _timeOfExpanding;
        float _decreasingTimer = _holdingTimer - _timeOfHolding;

        if (_increasingTimer <= _timeOfExpanding)
        {
            _lightRange = Mathf.Lerp(_maxLightRange, _maxLightRange + _lightExplosionRangeAdded, _increasingTimer / _timeOfExpanding);
        }
        else if (_holdingTimer <= _timeOfHolding)
        {
            //Do Nothing
        }
        else if (_decreasingTimer <= _timeOfDecreasing)
        {
            _lightRange = Mathf.Lerp(_maxLightRange + _lightExplosionRangeAdded, _maxLightRange, (_decreasingTimer) / _timeOfDecreasing);
            _lightRange += -1 - Time.fixedDeltaTime;
        }

        _light.pointLightOuterRadius = _lightRange;
        _explosionTimer += Time.fixedDeltaTime;
    }
    /* ----- HEALTH AND DAMAGE HERE  ------- */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsAttached()) return;

        IDamageFire water = collision.GetComponent<IDamageFire>();
        if (water != null)
        {
            TakeDamage(water.DamageDealt);
        }
    }

    public void TakeDamage(float damageDealt)
    {
        Debug.Log("FireDamage");
        _currentFireHealth -= damageDealt;
        Debug.Log("health: " + _currentFireHealth);
        if (_currentFireHealth < 0)
        {
            _currentFireHealth = 0;
            _fireParticlesPrefab.SetActive(false);
        }
    }

    private void OnCheckPointActivated()
    {
        HealMaximum();
        StartExplosion();
    }

    private void StartExplosion()
    {
        _explosionTimer = 0;
        _isExploding = true;
        _tremblingValue += 0.5f;
    }

    private void HealMaximum()
    {
        _currentFireHealth = _maxFireHealth;
    }

    public void RestartLevel()
    {
        HealMaximum();
        BePickedUp();
        AdjustLightEffect();
        _fireParticlesPrefab.SetActive(true);
    }
}
