using System;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class WaterDrop : MonoBehaviour, IDamageFire
{
    static Thrower _player;
    public float DamageDealt => Damage;
    
    [SerializeField]
    float Damage = 2;
    [SerializeField]
    float GravityTweak = 1;
    [SerializeField]
    AnimationClip _splatterClip;

    Rigidbody2D _rb;

    [SerializeField] GameObject _particlesPrefab;
    
    private enum States { Falling, Breaking }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
    }

    public void Init()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.gravityScale *= GravityTweak;
    }

    private void Update()
    {
        if (!GetComponent<SpriteRenderer>().isVisible)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _rb.simulated = false;
        InstantiateParticles();
        StartCoroutine(DestroyAtEndFrame());
    }

    private void InstantiateParticles()
    {
        Instantiate(_particlesPrefab, transform.position, Quaternion.identity);
    }

    IEnumerator DestroyAtEndFrame()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
