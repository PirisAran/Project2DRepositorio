using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerController Player;
    [SerializeField]
    Animator Animator;

    private void Awake()
    {
        Player = GetComponent<PlayerController>();
    }
    void Update()
    {
        //Animator manager provisional
        if (Player.Speed != 0)
        {
            Animator.SetBool("Running", true);
            var bodyScale = transform.localScale;
            bodyScale.x = Mathf.Abs(bodyScale.x) * Mathf.Sign(Player.Speed);
            transform.localScale = bodyScale;
        }   
        else
            Animator.SetBool("Running", false);

        Animator.SetBool("Jumping", Player.IsJumping);
    }


}
