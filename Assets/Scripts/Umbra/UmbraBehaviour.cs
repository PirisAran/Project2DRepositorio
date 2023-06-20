using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class UmbraBehaviour : MonoBehaviour, IRestartLevelElement
{
    GameObject _player;
    FireController _fire;

    [Header("FSM Variables")]
    [SerializeField]States _currentState;
    States _nextState;
    [SerializeField]float _followStateRange = 3f;
    [SerializeField]float _transitionTime = 1f;
    public float TransitionTime => _transitionTime;
    float _timeCurrentState;
    float _speedBeforeTransition;
    bool _permaKiller = false;
    [Space]

    [Header("States Base Speed")]
    [SerializeField] float _cuteBaseSpeed = 0f;
    [SerializeField] float _followBaseSpeed = 1f;
    [SerializeField] float _killerBaseSpeed = 6f;
    [Space]

    [Header("States Maximum Speed")]
    [SerializeField]float _cuteMaxSpeed = 2f;
    [SerializeField]float _followMaxSpeed = 10f;
    [SerializeField]float _killerMaxSpeed = 10f;
    [Space]

    [Header("Movement Values")]
    [SerializeField]float _currentSpeed;
    [SerializeField]float _acceleration;

    [Header("Umbra Sounds")]
    [SerializeField] SoundPlayer _huntingNoFire;
    [SerializeField] SoundPlayer _breath;
    [SerializeField] SoundPlayer _gettingOutLight;


    Vector3 _desiredPosition;

    public Action OnEnterCuteState;
    public Action OnEnterFollowState;
    public Action OnEnterKillerState;
    public Action OnEnterTransitionState;
    public Action OnPlayerStartKill;
    public Action OnPlayerFinishKill;


    public Vector3 Forward => (_desiredPosition - transform.position).normalized;
    public States CurrentState => _currentState;
    public float Speed => _currentSpeed;
    private Vector3 _playerDirection => (_player.transform.position - transform.position).normalized;
    private Vector3 _fireDirection => (_fire.transform.position - transform.position).normalized;
    private float _lightRange => _fire.LightRange;
    private float _respectRange => _fire.LightRange + _followStateRange;


    private AudioSource _breathAS;
    private float _oVolumeBreath;

    private bool _playerDead = false;
    //FSM States
    public enum States { Cute, Follow, Killer, Transition}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_playerDead)
        {
            return;
        }

        if (_currentState == States.Cute || _currentState == States.Transition) return;

        HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            StartCoroutine(KillPlayerCoroutine(healthSystem));
        }
    }

    private IEnumerator KillPlayerCoroutine(HealthSystem hs)
    {
        _playerDead = true;
        Debug.Log("umbra kills player");
        OnPlayerStartKill?.Invoke();
        yield return new WaitForSeconds(0.25f);
        OnPlayerFinishKill?.Invoke();
        hs.KillPlayer();
    }


    private void OnEnable()
    {
        FireController.OnFireDestroyed += OnFireDestroyed;
    }

    private void OnDisable()
    {
        FireController.OnFireDestroyed -= OnFireDestroyed;
    }
    private void Awake()
    {
    }

    private void Start()
    {
        Init();
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.gameObject;
        _fire = _player.GetComponentInChildren<FireController>();
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
        _breathAS = _breath.PlaySound().GetComponent<AudioSource>();
        _oVolumeBreath = _breathAS.volume;
    }
    private void Init()
    {
        _currentState = States.Follow;
        OnEnterFollowState?.Invoke();
        _timeCurrentState = 0;
    }

    private void FixedUpdate()
    {
        switch (_currentState)
        {
            case States.Cute:
                UpdateCuteState();
                break;
            case States.Follow:
                UpdateFollowState();
                break;
            case States.Killer:
                UpdateKillerState();
                break;
            case States.Transition:
                UpdateTransitionState();
                break;
            default:
                break;
        }
        _timeCurrentState += Time.deltaTime;
    }

    private void OnEnterState(States state)
    {
        switch (state)
        {
            case States.Cute:
                _gettingOutLight.PlaySound();
                OnEnterCuteState?.Invoke();
                break;
            case States.Follow:
                Debug.Log("follow");
                StartCoroutine(RandomBreathSound());
                OnEnterFollowState?.Invoke();
                break;
            case States.Killer:
                _huntingNoFire.PlaySound();
                OnEnterKillerState?.Invoke();
                break;
            case States.Transition:
                OnEnterTransitionState?.Invoke();
                break;
            default:
                break;
        }
    }

    private void TransitionToState()
    {
        _currentState = States.Transition;
        OnEnterState(States.Transition);
        _speedBeforeTransition = _currentSpeed;
        _timeCurrentState = 0;
    }

    private void UpdateCuteState()
    {
        //Antes de nada, comprovar si puede cambiar de estados
        if (CanExitCuteState())
            TransitionToState();
        
        _desiredPosition = _fire.transform.position + (-_fireDirection) * _respectRange;
        MoveTowardsPosition(_desiredPosition, _cuteBaseSpeed, _cuteMaxSpeed, _acceleration);
    }


    private void UpdateFollowState()
    {
        //Antes de nada, comprovar si puede cambiar de estados
        if (CanExitFollowState())
            TransitionToState();

        _desiredPosition = _player.transform.position + (-_playerDirection) * _respectRange;
        MoveTowardsPosition(_desiredPosition, _followBaseSpeed, _followMaxSpeed, _acceleration);
    }


    private void UpdateKillerState()
    {
        //Antes de nada, comprovar si puede cambiar de estados
        if (CanExitKillerState())
            TransitionToState();

        _desiredPosition = _player.transform.position;
        MoveTowardsPosition(_desiredPosition, _killerBaseSpeed, _killerMaxSpeed, _acceleration);
    }
    private void UpdateTransitionState()
    {
        if (_timeCurrentState >= _transitionTime)
        {
            if (CanEnterCuteState())
                _nextState = States.Cute;
            else if (CanEnterFollowState())
                _nextState = States.Follow;
            else
            {
                _nextState = States.Killer;
            }
            _currentState = _nextState;
            OnEnterState(_currentState);
            return;
        }
        float desiredSpeed = 0;
        float timeToDecelerate = _transitionTime;
        _currentSpeed = Mathf.Lerp(_speedBeforeTransition, desiredSpeed, Mathf.Clamp01(_timeCurrentState / timeToDecelerate));
        transform.Translate(Forward * _currentSpeed * Time.fixedDeltaTime);
    }
    private bool CanExitCuteState()
    {
        return CanEnterFollowState() || CanEnterKillerState() || _permaKiller;
    }

    private bool CanExitFollowState()
    {
        return (CanEnterCuteState() || CanEnterKillerState()) || _permaKiller;
    }
    private bool CanExitKillerState()
    {
        return CanEnterCuteState() || CanEnterFollowState();
    }


    private void MoveTowardsPosition(Vector3 desiredPosition, float baseSpeed, float maxSpeed, float acceleration)
    {
        float distance = Vector3.Distance(transform.position, desiredPosition);

        // calcular la velocidad actual del objeto
        _currentSpeed = baseSpeed + acceleration * distance;

        // limitar la velocidad máxima del objeto
        _currentSpeed = Mathf.Clamp(_currentSpeed, baseSpeed, maxSpeed);

        // mover el objeto hacia la posición del mouse con velocidad acelerada
        //transform.position = Vector3.MoveTowards(transform.position, desiredPosition, _currentSpeed * Time.deltaTime);
        float mov = _currentSpeed * Time.fixedDeltaTime;

        if (mov > distance) mov = distance;

        transform.Translate(Forward * mov);
    }
    private bool IsInLightRange()
    {
        return Vector3.Distance(transform.position, _fire.transform.position) < _lightRange && _lightRange > 0;
    }
    private bool IsPlayerSafe()
    {
        return Vector3.Distance(_player.transform.position, _fire.transform.position) <= _lightRange && _lightRange > 0;
    }

    private bool CanEnterCuteState()
    {
        return IsInLightRange() && !_permaKiller;
    }
    private bool CanEnterFollowState()
    {
        return !IsInLightRange() && IsPlayerSafe() && !_permaKiller;
    }
    private bool CanEnterKillerState()
    {
        return !IsInLightRange() && !IsPlayerSafe() || _fire.LightRange <= 0 || _permaKiller;
    }

    public void RestartLevel()
    {
        transform.position = GameLogic.GetGameLogic().GetGameController().GetLevelController().GetUmbraSpawnPoint().position;
        _permaKiller = false;
        _playerDead = false;
    }

    private void OnFireDestroyed()
    {
        _permaKiller = true;
        TransitionToState();
    }

    IEnumerator RandomBreathSound()
    {

        float minSoundingTime = 4;
        float maxSoundingTime = 6;

        float minNotSoundingTime = 2;
        float maxNotSoundingTime = 4;

        float fadeTime = 2f;
        float timer = 0;


        while (true)
        {
            //VERSION VOLUMEN BAJA DE GOLPE
            //_breathAS.volume = _oVolumeBreath;
            //yield return new WaitForSeconds(UnityEngine.Random.Range(minSoundingTime, maxSoundingTime));
            //_breathAS.volume = 0;
            //yield return new WaitForSeconds(UnityEngine.Random.Range(minNotSoundingTime, maxNotSoundingTime));

            timer = fadeTime;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                _breathAS.volume = Mathf.Lerp(0, _oVolumeBreath, Mathf.Clamp01((fadeTime - timer) / fadeTime));
                yield return null;
            }
            _breathAS.volume = _oVolumeBreath;

            yield return new WaitForSeconds(UnityEngine.Random.Range(minSoundingTime, maxSoundingTime));
            timer = fadeTime;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                _breathAS.volume = Mathf.Lerp(_oVolumeBreath, 0, Mathf.Clamp01((fadeTime - timer) / fadeTime));
                yield return null;
            }
            _breathAS.volume = 0;

            yield return new WaitForSeconds(UnityEngine.Random.Range(minNotSoundingTime, maxNotSoundingTime));
        }
    }
}
