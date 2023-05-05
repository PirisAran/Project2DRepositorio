using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxVelocity : MonoBehaviour
{
    [SerializeField]
    Vector2 Velocity;
    [SerializeField]
    Rigidbody2D Target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Target.velocity.y<Velocity.y)
        {
            Target.velocity = Velocity;
        }
        Debug.Log("targer velociy:"+Target.velocity);
    }
}
