using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Transform child;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(0);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (child.parent != null)
        {
            if (rb.velocity.x < 0)
                child.localPosition = new Vector2(-1.5f, 0);
            else if (rb.velocity.x > 0)
                child.localPosition = new Vector2(1.5f, 0);
        }
    }
}
