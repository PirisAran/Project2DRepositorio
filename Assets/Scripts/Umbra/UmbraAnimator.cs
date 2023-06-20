using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TecnocampusProjectII;

public class UmbraAnimator : MonoBehaviour, IRestartLevelElement
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

    //[SerializeField] Transform _transformationParticlesPrefabTransform;
    [SerializeField] GameObject _transformationParticlesPrefab;
    ParticleSystem _particleSystem;
    ParticleSystem.EmissionModule _particleSystemEmission;

    UmbraBehaviour _umbraFSM;

    IEnumerator _currentTransformation;

    bool _playerDead = false;

    private void Awake()
    {
        _umbraFSM = GetComponent<UmbraBehaviour>();

        //GameObject _particlesUmbra = Instantiate(_transformationParticlesPrefab, _transformationParticlesPrefabTransform.position, _transformationParticlesPrefab.transform.rotation);
        //_particleSystem = _particlesUmbra.GetComponent<ParticleSystem>();
        //_particleSystemEmission = _particleSystem.emission;
        _particleSystem = _transformationParticlesPrefab.GetComponent<ParticleSystem>();
        _particleSystemEmission = _particleSystem.emission;
        _particleSystemEmission.enabled = false;

        _allBones.Add(_cuteBones);
        _allBones.Add(_followBones);
        _allBones.Add(_killerBones);
    }

    private void Start()
    {
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
    }

    private void OnEnable()
    {
        _umbraFSM.OnEnterCuteState += OnCuteState;
        _umbraFSM.OnEnterFollowState += OnFollowState;
        _umbraFSM.OnEnterKillerState += OnKillerState;
        _umbraFSM.OnEnterTransitionState += OnTransitionState;
        _umbraFSM.OnPlayerStartKill += OnKillPlayer;
        _umbraFSM.OnPlayerFinishKill += OnPlayerFinishKill;
    }

    private void OnDisable()
    {
        _umbraFSM.OnEnterCuteState -= OnCuteState;
        _umbraFSM.OnEnterFollowState -= OnFollowState;
        _umbraFSM.OnEnterKillerState -= OnKillerState;
        _umbraFSM.OnEnterTransitionState -= OnTransitionState;
        _umbraFSM.OnPlayerStartKill -= OnKillPlayer;
        _umbraFSM.OnPlayerFinishKill -= OnPlayerFinishKill;
    }


    private void FixedUpdate()
    {
        if (_currentAnim == null) return;
        _currentAnim.SetBool("moving", _umbraFSM.Speed > 1.5f);

        if (_umbraFSM.CurrentState == UmbraBehaviour.States.Transition) return;
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

    void OnKillPlayer()
    {
        _playerDead = true;
        SetCurrentAnimator(_killerBones);   
        _killerBones.SetBool("attack", true);
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
        _particleSystem.Emit(15);
        yield return new WaitForSeconds(_umbraFSM.TransitionTime);
        _currentTransformation = null;
    }

    private void OnPlayerFinishKill()
    {
        _killerBones.SetBool("attack", false);
    }

    public void RestartLevel()
    {
        _playerDead = false;
    }
}
