using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour, IDamageFire
{
    public float DamageDealt => Damage;
    
    [SerializeField]
    float Damage = 2;

    [SerializeField]
    float GravityTweak = 1;

    Rigidbody2D _rb;
    Animator _animator;
    ParticleSystem _particleSystem;
    
    private enum States { Falling, Breaking }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _particleSystem = GetComponent<ParticleSystem>();

        
    }

    public void Init()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.gravityScale *= GravityTweak;
    }

    private void Update()
    {
        if (!GetComponent<SpriteRenderer>().isVisible)
            Destroy();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _particleSystem.Play(true);
        _animator.SetBool("Splat", true);
        transform.Translate(Vector2.up * 0.0f);
        _rb.simulated = false;
    }
}
