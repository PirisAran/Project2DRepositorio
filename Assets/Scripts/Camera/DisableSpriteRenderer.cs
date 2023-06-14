using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSpriteRenderer : MonoBehaviour
{
    [SerializeField] bool _activated = true;

    private void Awake()
    {
        if (!_activated) return;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
