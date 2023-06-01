using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyableObject obj = collision.gameObject.GetComponent<DestroyableObject>();

        if (obj != null)
        {
            CallDestroyObject(obj);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyableObject obj = collision.gameObject.GetComponent<DestroyableObject>();

        if (obj != null)
        {
            CallDestroyObject(obj);
        }
    }
    private void CallDestroyObject(DestroyableObject obj)
    {
        obj.DestroyObject();
    }
}
