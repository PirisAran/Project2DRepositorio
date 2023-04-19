using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLive : MonoBehaviour
{
    public static Action<SpiderLive> OnSpiderwebDestroyed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Candle>())
        {
            Debug.Log("Spiderweb destroyed");
            SpiderwebDestroyed();
        }
    }

    private void SpiderwebDestroyed()
    {
        OnSpiderwebDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
