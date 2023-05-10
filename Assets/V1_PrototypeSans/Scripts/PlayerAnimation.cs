using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer SpriteRenderer;
    PlayerController Player;

    private void Awake()
    {
    }
    void Update()
    {
        if (Player.Forward.x != 0)
            SpriteRenderer.flipX = (Player.Forward.x < 0); 
    }
}
