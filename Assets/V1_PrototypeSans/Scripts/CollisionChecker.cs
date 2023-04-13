using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    Jumper _jumper;


    [SerializeField]
    private float DetectionRadius = 0.15f;

    [SerializeField]
    Transform GroundCheckerOrigin;


    public bool CanJump => _canJump;
    bool _canJump;

    private void Awake()
    {
        _jumper = GetComponentInParent<Jumper>();
    }
    private void FixedUpdate()
    {
        _canJump = false;

        CheckIfOnGround();
    }

    private void CheckIfOnGround()
    {
        var colliders = Physics2D.OverlapCircleAll(GroundCheckerOrigin.position, DetectionRadius);
        _canJump = (colliders.Length > 0);
    }

}
