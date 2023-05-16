using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField]
    private Vector2 _boxSize = new Vector2(1.25f, 0.1f);
    [SerializeField]
    LayerMask _whatIsCollision;
    [SerializeField]
    Transform _groundCheckerOrigin;
    [SerializeField]
    float _coyoteTime = 0.2f;
    float _timer;

    public Action OnLanding;

    public bool Colliding => _colliding;
    bool _colliding;

    bool lastOnGround = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_groundCheckerOrigin.position, _boxSize);
    }

    private void Update()
    {
        //if (CheckOnGround())
        //{
        //    _timer = 0;
        //}

        //if (_timer <= _coyoteTime)
        //{
        //    _colliding = true;      
        //}

        _colliding = CheckOnGround();

        CheckLanding();
    }

    private bool CheckOnGround()
    {
        _colliding = false;
        var colliders = Physics2D.OverlapBoxAll(_groundCheckerOrigin.position, _boxSize, 0, _whatIsCollision);
        return (colliders.Length > 0);
    }

    private void CheckLanding()
    {
        var currentOnGround = _colliding;

        if (!lastOnGround && currentOnGround)
            OnLanding?.Invoke();

        lastOnGround = currentOnGround;
    }

}
