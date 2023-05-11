using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    GameObject Body;
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
        }
        else
            Animator.SetBool("Running", false);

        Animator.SetBool("Jumping", Player.IsJumping);
    }


}
