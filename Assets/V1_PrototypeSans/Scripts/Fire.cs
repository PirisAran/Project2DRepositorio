using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Fire : MonoBehaviour
{
    [SerializeField]
    float PickUpRadius = 1;
    [SerializeField]
    float MaxLightRange = 6;
    [SerializeField]
    CircleCollider2D PickUpCollider;
    [SerializeField]
    CircleCollider2D DamageCollider;
    [SerializeField]
    Light2D Light;
    [SerializeField]
    Color Color;
    [SerializeField]
    Transform Player;
    [SerializeField]
    GameObject FireParticles;
    [SerializeField]
    float MaxFireHealth = 10;
    SpriteRenderer _spriteRenderer;

    public float LightRange => Light.pointLightOuterRadius;
    float _currentFireHealth;
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageFire water = other.GetComponent<IDamageFire>();
        if(water != null)
        {
            TakeDamage(water.DamageDealt);
            water.Destroy();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Light.pointLightOuterRadius);
        Gizmos.color = Color.white;
    }

    public void TakeDamage(float damageDealt)
    {
        Debug.Log("fireDamaged:" + damageDealt);
        _currentFireHealth -= damageDealt;
        AdjustLight(Mathf.Clamp01(_currentFireHealth/MaxFireHealth));
    }

    private void AdjustLight(float fraction)
    {
        Light.pointLightOuterRadius = Mathf.Lerp(0, MaxLightRange, fraction);
    }

    public bool IsAttached => _isAttached;
    bool _isAttached;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform == Player)
            GetComponent<Collider2D>().isTrigger = false;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        InitLight();
    }

    private void InitLight()
    {
        PickUpCollider.radius = PickUpRadius;
        Light.pointLightInnerRadius = 0;
        Light.pointLightOuterRadius = MaxLightRange;
        Light.color = Color;
        _currentFireHealth = MaxFireHealth;
        AttachToPlayer();
    }

    private void Update()
    {
        if (_isAttached)
            transform.localPosition = Vector2.zero;

    }

    internal void AttachToPlayer()
    {
        transform.SetParent(Player);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().isTrigger = true;
        _isAttached = true;
        Hide();
    }

    internal void DetachFromPlayer()
    {
        transform.SetParent(null);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        _isAttached = false;
        Show();
    }

    private void Hide()
    {
        FireParticles.SetActive(false);
        _spriteRenderer.enabled = false;
    }
    void Show()
    {
        FireParticles.SetActive(true);
        _spriteRenderer.enabled = true;
    }
}
