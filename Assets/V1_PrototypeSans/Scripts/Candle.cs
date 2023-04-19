using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Candle : MonoBehaviour
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

    private void OnEnable()
    {
        SpiderLive.OnSpiderwebDestroyed += OnCandleCollidesWithSpideweb;
    }

    private void OnDisable()
    {
        SpiderLive.OnSpiderwebDestroyed -= OnCandleCollidesWithSpideweb;
    }
    private void OnCandleCollidesWithSpideweb(SpiderLive obj)
    {
        
    }

    private void Awake()
    {

        InitCandle();
    }

    private void InitCandle()
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
