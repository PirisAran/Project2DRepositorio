using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other);
        Debug.Log("Destruido: " + other);
    }
}
