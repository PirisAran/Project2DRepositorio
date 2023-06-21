using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundChecker : CollisionChecker
{
    [SerializeField]
    private Vector2 _boxSize = new Vector2(1.25f, 0.1f);
    public bool OnGround => _colliding;

    [SerializeField] LayerMask _whatIsGeyser;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_groundCheckerOrigin.position, _boxSize);
    }

    private void Update()
    {

        _colliding = CheckIfColliding();
        CheckLanding();
    }

   

    protected override bool CheckIfColliding()
    {
        _colliding = false;
        var colliders = Physics2D.OverlapBoxAll(_groundCheckerOrigin.position, _boxSize, 0, _whatIsCollision);
        return (colliders.Length > 0);
    }
}
