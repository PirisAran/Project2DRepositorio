using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UmbraAnimator : MonoBehaviour
{
    [SerializeField]
    Sprite CuteSprite;
    [SerializeField]
    Sprite ChillSprite;
    [SerializeField]
    Sprite ChasingSprite;

    UmbraFSM _umbraFSM;
    V2UmbraFSM _V2umbraFSM;
    SpriteRenderer _spriteRenderer;

    [SerializeField]
    List<Transform> EyeLights;

    private void Awake()
    {
        _umbraFSM = GetComponent<UmbraFSM>();
        _V2umbraFSM = GetComponent<V2UmbraFSM>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _umbraFSM.OnCuteState += OnCuteState;
        _umbraFSM.OnChillState += OnChillState;
        _umbraFSM.OnChasingState += OnChasingState;

        _V2umbraFSM.OnHarmlessState += OnCuteState;
        _V2umbraFSM.OnChasingState += OnChillState;
        _V2umbraFSM.OnKillerState += OnChasingState;
    }

    private void OnDisable()
    {
        _umbraFSM.OnCuteState -= OnCuteState;
        _umbraFSM.OnChillState -= OnChillState;
        _umbraFSM.OnChasingState -= OnChasingState;

        _V2umbraFSM.OnHarmlessState -= OnCuteState;
        _V2umbraFSM.OnChasingState -= OnChillState;
        _V2umbraFSM.OnKillerState -= OnChasingState;
    }

    private void Update()
    {
        //_spriteRenderer.flipX = _umbraFSM.PlayerDirection.x < 0;
    }

    void ChangeSprite(Sprite nextSprite)
    {
        _spriteRenderer.sprite = nextSprite;
    }

    void OnCuteState()
    {
        ChangeSprite(CuteSprite);
    }

    void OnChillState()
    {
        ChangeSprite(ChillSprite);
    }

    void OnChasingState()
    {
        ChangeSprite(ChasingSprite);
    }
}
