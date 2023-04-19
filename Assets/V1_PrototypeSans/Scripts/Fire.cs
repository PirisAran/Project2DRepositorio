using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Fire : MonoBehaviour
{
    [SerializeField]
    float PickUpRadius = 1;

    [SerializeField]
    float MaxLightRange = 6;

    [SerializeField]
    CircleCollider2D PickUpCollider;

    [SerializeField]
    Light2D Light;

    [SerializeField]
    Color Color;

    [SerializeField]


    private void Awake()
    {

        InitFire();
    }

    private void InitFire()
    {
        PickUpCollider.radius = PickUpRadius;
        Light.pointLightInnerRadius = 0;
        Light.pointLightOuterRadius = MaxLightRange;
        Light.color = Color;
    }

    private void Update()
    {

    }

}
