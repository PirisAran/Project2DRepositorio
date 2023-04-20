using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer SpriteRenderer;
    [SerializeField]
    PlayerMovement PlayerMovement;

    private void Awake()
    {
    }
    void Update()
    {
        if (PlayerMovement.Direction.x != 0)
            SpriteRenderer.flipX = (PlayerMovement.Direction.x < 0); 
    }
}
