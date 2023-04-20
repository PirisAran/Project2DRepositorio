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
    Light2D Light;

    [SerializeField]
    Color Color;

    [SerializeField]
    Transform Player;

    [SerializeField]
    GameObject Particles;

    bool _isAttached;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform == Player)
            GetComponent<Collider2D>().isTrigger = false;
    }

    private void Awake()
    {
        InitFire();
    }

    private void InitFire()
    {
        PickUpCollider.radius = PickUpRadius;
        Light.pointLightInnerRadius = 0;
        Light.pointLightOuterRadius = MaxLightRange;
        Light.color = Color;
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
        Particles.SetActive(false);
    }
    void Show()
    {
        Particles.SetActive(true);
    }
}
