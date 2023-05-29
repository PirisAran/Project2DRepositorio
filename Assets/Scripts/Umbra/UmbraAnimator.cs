using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UmbraAnimator : MonoBehaviour
{
    //[SerializeField]
    //Sprite _cuteSprite;
    //[SerializeField]
    //Sprite _followSprite;
    //[SerializeField]
    //Sprite _killerSprite;
    //[SerializeField]
    //Sprite _transitionSprite;

    [SerializeField] Animator _cuteBones, _followBones, _killerBones;
    Animator _currentAnim;

    List<Animator> _allBones = new List<Animator>();

    [SerializeField]
    GameObject _transformationParticles;

    UmbraFSM _umbraFSM;

    IEnumerator _currentTransformation;

    private void Awake()
    {
        _umbraFSM = GetComponent<UmbraFSM>();
        _transformationParticles.SetActive(false);
        _allBones.Add(_cuteBones);
        _allBones.Add(_followBones);
        _allBones.Add(_killerBones);
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

    private void FixedUpdate()
    {
        if (_currentAnim == null) return;
        _currentAnim.SetBool("moving", _umbraFSM.Speed > 1.5f);
        Debug.Log(_umbraFSM.Speed);

        if (_umbraFSM.CurrentState == UmbraFSM.States.Transition) return;
        var scale = _currentAnim.transform.localScale;
        _currentAnim.transform.localScale = new Vector2(Mathf.Abs(scale.x) * Mathf.Sign(_umbraFSM.Forward.x), scale.y);
    }   

    void OnCuteState()
    {
        SetCurrentAnimator(_cuteBones);
    }
    void OnFollowState()
    {
        SetCurrentAnimator(_followBones);
    }
    void OnKillerState()
    {
        SetCurrentAnimator(_killerBones);
    }
    void OnTransitionState()
    {
        StartTransformationEffect();
    }

    private void StartTransformationEffect()
    {
        if (_currentTransformation != null)
        {
            StopCoroutine(_currentTransformation);
            _currentTransformation = null;
        }
        _currentTransformation = DoTransformationAnimation();
        StartCoroutine(_currentTransformation);
    }

    private void SetCurrentAnimator(Animator anim)
    {
        foreach (Animator animator in _allBones)
            animator.gameObject.SetActive(false);
        _currentAnim = anim;
        _currentAnim.gameObject.SetActive(true);
    }

    private IEnumerator DoTransformationAnimation()
    {
        _transformationParticles.SetActive(true);
        yield return new WaitForSeconds(_umbraFSM.TransitionTime + 0.25f);
        _transformationParticles.SetActive(false);
        _currentTransformation = null;
    }
}
