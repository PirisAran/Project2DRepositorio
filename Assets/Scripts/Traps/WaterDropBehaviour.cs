using System;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class WaterDropBehaviour : MonoBehaviour
{
    [Header("Scripts Utilizados")]
    [SerializeField] FireDamager _damageFire;
    [SerializeField] Spawner _spawner;
    [SerializeField] SoundPlayer _soundPlayer;

    [Space]
    [SerializeField]
    float GravityTweak = 1;
    [SerializeField]
    AnimationClip _splatterClip;

    Rigidbody2D _rb;
    
    private enum States { Falling, Breaking }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Init();
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
        Debug.Log(_soundPlayer == null);
        _soundPlayer.PlaySound();
        StartCoroutine(DestroyAtEndFrame());
    }

    private void InstantiateParticles()
    {
        _spawner.SpawnOne(transform.position, Quaternion.identity);
    }

    IEnumerator DestroyAtEndFrame()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
