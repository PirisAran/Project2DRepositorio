using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class FireController : MonoBehaviour
{
                                             /* ---------- FIRE CONTROLLER ----------- */
    [SerializeField] Thrower _playerThrower;
    
    //Components    
    [SerializeField] List<SpriteRenderer> _sprites;
    Rigidbody2D _rb;
    FireGroundChecker _collCheck;

    [Header ("Light Parameters")]
    [SerializeField]
    Light2D _light;
    [SerializeField]
    float _maxLightRange = 6;
    [SerializeField]
    Color _lightColor;
    [SerializeField]
    GameObject _fireParticlesPrefab;
    public float LightRange => _lightRange;
    float _lightRange;
    [Space] 

    [Header("Light Effect Parameters")]
    [SerializeField] float _tremblingValue = 0.5f;
    [SerializeField] float _intervalTimeMax = 0.15f, _intervalTimeMin = 0.05f;
    float _lastTimeTremble;
    [SerializeField] float _lightExplosionRangeAdded = 5;
    [SerializeField] float _timeOfExpanding = 0.3f, _timeOfHolding = 0.3f, _timeOfDecreasing = 0.6f;
    float _explosionTimer;
    bool _isExploding = false;

    ParticleSystem _particleSystem;
 


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
    [SerializeField]
    CircleCollider2D _damageCollider;

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
    }
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _pickUpCollider.radius = _pickUpRadius;
        _lightRange = _maxLightRange;
        _light.pointLightOuterRadius = _lightRange;
        _currentFireHealth = _maxFireHealth;
        _light.color = _lightColor;
        BePickedUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            transform.localPosition = Vector2.zero;
        }

        if (_isExploding)
        {
            UpdateExplosionEffect();
        }
        UpdateLightEffect();
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
        Hide();
    }

    private void SetAttached(bool v)
    {
        _playerThrower.SetHasFire(v);
        transform.parent = v ? _playerThrower.gameObject.transform : null;
        _rb.bodyType = v ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
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
    private void UpdateLightEffect()
    {
        if (_currentFireHealth <= 0)
        {
            _lightRange = 0;
            return;
        }
        if (Time.time - _lastTimeTremble >= Random.Range(_intervalTimeMin, _intervalTimeMax))
        {
            var tempLightRange = LightRange + Random.Range(-_tremblingValue, _tremblingValue);
            _light.pointLightOuterRadius = tempLightRange;
            _lastTimeTremble = Time.time;
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
    private void AdjustLight(float fraction)
    {
        _lightRange = Mathf.Lerp(0, _maxLightRange, fraction);
        _light.pointLightOuterRadius = _lightRange;
        var particleSystem = _fireParticlesPrefab.GetComponent<ParticleSystem>();
    }

    private void UpdateExplosionEffect()
    {
        if (_explosionTimer >= _timeOfDecreasing + _timeOfExpanding + _timeOfHolding)
        {
            _isExploding = false;
            AdjustLight(_currentFireHealth / _maxFireHealth);
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
        _explosionTimer += Time.fixedDeltaTime;
    }
    /* ----- HEALTH AND DAMAGE HERE  ------- */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageFire water = collision.GetComponent<IDamageFire>();
        if (water != null)
        {
            TakeDamage(water.DamageDealt);
        }
    }

    private void TakeDamage(float damageDealt)
    {
        Debug.Log("FireDamage");
        _currentFireHealth -= damageDealt;
        Debug.Log("health: " + _currentFireHealth);
        if (_currentFireHealth < 0)
        {
            _currentFireHealth = 0;
            _fireParticlesPrefab.SetActive(false);
        }
        AdjustLight(Mathf.Clamp01(_currentFireHealth / _maxFireHealth));
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
        AdjustLight(Mathf.Clamp01(_currentFireHealth / _maxFireHealth));
    }


}
