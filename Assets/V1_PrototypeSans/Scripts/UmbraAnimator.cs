using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UmbraAnimator : MonoBehaviour
{
    [SerializeField]
    Sprite CuteSprite;
    [SerializeField]
    Sprite ChasingSprite;
    [SerializeField]
    Sprite KillerSprite;

    UmbraFSM _umbraFSM;
    UmbraController _umbraController;
    SpriteRenderer _spriteRenderer;

    [SerializeField]
    List<Transform> EyeLights;

    private void Awake()    
    {
        _umbraController = GetComponent<UmbraController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _umbraController.OnCuteState += OnCuteState;
        _umbraController.OnChasingState += OnChasingState;
        _umbraController.OnKillerState += OnKillerState;
    }

    private void OnDisable()
    {
        _umbraController.OnCuteState -= OnCuteState;
        _umbraController.OnChasingState -= OnChasingState;
        _umbraController.OnKillerState -= OnKillerState;
    }

    private void Update()
    {
    }

    void ChangeSprite(Sprite nextSprite)
    {
        _spriteRenderer.sprite = nextSprite;
    }

    void OnCuteState()
    {
        ChangeSprite(CuteSprite);
    }
    void OnChasingState()
    {
        ChangeSprite(ChasingSprite);
    }
    void OnKillerState()
    {
        ChangeSprite(KillerSprite);
    }
}
