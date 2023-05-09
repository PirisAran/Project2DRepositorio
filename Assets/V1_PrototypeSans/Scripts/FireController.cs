using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    public float LightRange => Light.pointLightOuterRadius;

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
        Light.pointLightOuterRadius = MaxLightRange;
        _currentFireHealth = MaxFireHealth;
        Light.color = LightColor;
        BePickedUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAttached())
        {
            transform.localPosition = new Vector2(0, 0);
        }
    }

    /* ------ PICK UP AND BE THROWN ------ */
    public void BeThrown(Vector2 dir, float currentThrowSpeed)
    {
        transform.parent = null;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.velocity = dir * currentThrowSpeed;
        Show();
    }

    public void BePickedUp()
    {
        transform.parent = Player.transform;
        _rb.bodyType = RigidbodyType2D.Static;
        Hide();
    }

    private bool IsAttached()
    {
        return Player.HasFire;
    }

    private void OnLanding()
    {
        _rb.velocity = new Vector2(0, 0);
    }

    /* ----- ----- APPEARENCE (SHOW, HIDE, ETC) --------- */

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
        Light.pointLightOuterRadius = Mathf.Lerp(0, MaxLightRange, fraction);
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
