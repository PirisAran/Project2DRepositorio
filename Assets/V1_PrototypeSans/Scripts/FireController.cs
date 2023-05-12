using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class FireController : MonoBehaviour
{
                                             /* ---------- FIRE CONTROLLER ----------- */
    [SerializeField]
    PlayerController Player;
    
    //Components
    SpriteRenderer _spriteRd;
    Rigidbody2D _rb;
    CollisionChecker _collCheck;

    //Light Parameters-------------------
    [SerializeField]
    Light2D Light;
    [SerializeField]
    float MaxLightRange = 6;
    [SerializeField]
    Color LightColor;
    [SerializeField]
    GameObject FireParticles;
    public float LightRange => _lightRange;
    float _lightRange;
    //Light Effect Parameters (que la luz tiemble un poco que parezca fuego)
    [SerializeField]
    float tremblingValue = 0.5f;
    [SerializeField]
    float _intervalTimeMax = 0.15f, _intervalTimeMin = 0.05f;
    float _lastTimeTremble;


    //Pick up and throw parameters--------------------
    [SerializeField]
    CircleCollider2D PickUpCollider;
    [SerializeField]
    float PickUpRadius = 1;
    public bool OnPickUpRange => Player.PickUpCollider.IsTouching(PickUpCollider);
    
    //Health Parameters---------------------
    [SerializeField]
    float MaxFireHealth = 10;
    float _currentFireHealth;
    public float CurrentFireHealth { get { return _currentFireHealth;} set { _currentFireHealth = value;} }
    [SerializeField]
    CircleCollider2D DamageCollider;

    private void OnEnable()
    {
        _collCheck.OnLanding += OnLanding;
    }

    private void OnDisable()
    {
        _collCheck.OnLanding -= OnLanding;

    }
    private void Awake()
    {
        _spriteRd = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _collCheck = GetComponent<CollisionChecker>();
        Init();
    }

    private void Init()
    {
        PickUpCollider.radius = PickUpRadius;
        _lightRange = MaxLightRange;
        Light.pointLightOuterRadius = _lightRange;
        _currentFireHealth = MaxFireHealth;
        Light.color = LightColor;
        BePickedUp();
    }

    // Update is called once per frame
    void Update()
    {
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
        Player.SetHasFire(v);
        transform.parent = v ? Player.transform : null;
        _rb.simulated = !v;
    }

    private bool IsAttached()
    {
        return Player.HasFire;
    }

    private void OnLanding()
    {
        _rb.velocity = Vector2.zero;
    }

    /* ----- ----- APPEARENCE (SHOW, HIDE, ETC) --------- */
    private void UpdateLightEffect()
    {
        if (Time.time - _lastTimeTremble >= Random.Range(_intervalTimeMin, _intervalTimeMax))
        {
            var tempLightRange = LightRange + Random.Range(-tremblingValue, tremblingValue);
            Light.pointLightOuterRadius = tempLightRange;
            _lastTimeTremble = Time.time;
        }
    }
    private void Hide()
    {
        FireParticles.SetActive(false);
        _spriteRd.enabled = false;
    }
    void Show()
    {
        FireParticles.SetActive(true);
        _spriteRd.enabled = true;
    }
    private void AdjustLight(float fraction)
    {
        _lightRange = Mathf.Lerp(0, MaxLightRange, fraction);
        Light.pointLightOuterRadius = _lightRange;
    }

    /* ----- HEALTH AND DAMAGE HERE  ------- */

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageFire water = other.GetComponent<IDamageFire>();
        if (water != null)
        {
            TakeDamage(water.DamageDealt);
            water.Destroy();
        }
    }
    private void TakeDamage(float damageDealt)
    {
        _currentFireHealth -= damageDealt;
        AdjustLight(Mathf.Clamp01(_currentFireHealth / MaxFireHealth));
    }
}
