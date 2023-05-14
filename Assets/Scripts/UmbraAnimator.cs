using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UmbraAnimator : MonoBehaviour
{
    [SerializeField]
    Sprite _cuteSprite;
    [SerializeField]
    Sprite _followSprite;
    [SerializeField]
    Sprite _killerSprite;
    [SerializeField]
    Sprite _transitionSprite;

    UmbraFSM _umbraController;
    SpriteRenderer _spriteRenderer;

    private void Awake()    
    {
        _umbraController = GetComponent<UmbraFSM>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _umbraController.OnEnterCuteState += OnCuteState;
        _umbraController.OnEnterFollowState += OnFollowState;
        _umbraController.OnEnterKillerState += OnKillerState;
        _umbraController.OnEnterTransitionState += OnTransitionState;
    }

    private void OnDisable()
    {
        _umbraController.OnEnterCuteState -= OnCuteState;
        _umbraController.OnEnterFollowState -= OnFollowState;
        _umbraController.OnEnterKillerState -= OnKillerState;
        _umbraController.OnEnterTransitionState -= OnTransitionState;
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
        ChangeSprite(_cuteSprite);
    }
    void OnFollowState()
    {
        ChangeSprite(_followSprite);
    }
    void OnKillerState()
    {
        ChangeSprite(_killerSprite);
    }
    void OnTransitionState()
    {
        ChangeSprite(_transitionSprite);
    }
}
