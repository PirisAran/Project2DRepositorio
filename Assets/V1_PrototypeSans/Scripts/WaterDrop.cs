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

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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
        var fire = other.GetComponentInParent<FireController>();
        if (fire!=null)
        {
            fire.TakeDamage(Damage);
            Destroy();
        }
    }
}
