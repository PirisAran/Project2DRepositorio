using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField]
    private float DetectionRadius = 0.15f;

    [SerializeField]
    Transform GroundCheckerOrigin;

    public Action OnLanding;

    public bool OnGround => _onGround;
    bool _onGround;

    bool lastOnGround = false;

    private void Update()
    {
        CheckOnGround();
        CheckLanding();
    }

    private void CheckOnGround()
    {
        _onGround = false;
        var colliders = Physics2D.OverlapCircleAll(GroundCheckerOrigin.position, DetectionRadius);
        _onGround = (colliders.Length > 0);
    }

    private void CheckLanding()
    {
        var currentOnGround = _onGround;

        if (!lastOnGround && currentOnGround)
        {
            OnLanding?.Invoke();
            Debug.Log("landed");
        }

        lastOnGround = currentOnGround;
    }

}
