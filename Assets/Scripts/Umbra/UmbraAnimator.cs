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

    [SerializeField]
    GameObject _transformationParticles;

    UmbraFSM _umbraFSM;
    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _umbraFSM = GetComponent<UmbraFSM>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transformationParticles.SetActive(false);
    }

    private void OnEnable()
    {
        _umbraFSM.OnEnterCuteState += OnCuteState;
        _umbraFSM.OnEnterFollowState += OnFollowState;
        _umbraFSM.OnEnterKillerState += OnKillerState;
        _umbraFSM.OnEnterTransitionState += OnTransitionState;
    }

    private void OnDisable()
    {
        _umbraFSM.OnEnterCuteState -= OnCuteState;
        _umbraFSM.OnEnterFollowState -= OnFollowState;
        _umbraFSM.OnEnterKillerState -= OnKillerState;
        _umbraFSM.OnEnterTransitionState -= OnTransitionState;
    }

    private void Update()
    {
        if (_umbraFSM.Forward.x != 0 && _umbraFSM.CurrentState != UmbraFSM.States.Transition)
            _spriteRenderer.flipX = _umbraFSM.Forward.x < 0;
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
        StartTransformationEffect();
    }

    private void StartTransformationEffect()
    {
        StartCoroutine(DoTransformationAnimation());
    }

    private IEnumerator DoTransformationAnimation()
    {
        _transformationParticles.SetActive(true);
        yield return new WaitForSeconds(_umbraFSM.TransitionTime + 0.25f);
        _transformationParticles.SetActive(false);
    }
}
