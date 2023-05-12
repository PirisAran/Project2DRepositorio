using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CepoTrap : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == Player.transform)
        {
            Debug.Log("Atrapado");
        }
    }
}
