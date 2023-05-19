using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGroundChecker : CollisionChecker
{
    [SerializeField] float _detectionRadius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _colliding = CheckIfColliding();
        CheckLanding();
    }
    protected override bool CheckIfColliding()
    {
        _colliding = false;
        var collisions = Physics2D.OverlapCircleAll(_groundCheckerOrigin.position, _detectionRadius, _whatIsCollision);
        return collisions.Length > 0;
    }
}
