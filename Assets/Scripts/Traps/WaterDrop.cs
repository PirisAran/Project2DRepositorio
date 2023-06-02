using System;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    [Header("Scripts Utilizados")]
    [SerializeField] DamageFire _damageFire;

    [Space]
    [SerializeField]
    float GravityTweak = 1;
    [SerializeField]
    AnimationClip _splatterClip;

    [SerializeField] GameObject _particlesPrefab;
    Rigidbody2D _rb;
    
    private enum States { Falling, Breaking }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Init()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.gravityScale *= GravityTweak;
        _damageFire.SetCanDamage(true);
    }

    private void Update()
    {
        //if (!GetComponent<SpriteRenderer>().isVisible)
        //    Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
