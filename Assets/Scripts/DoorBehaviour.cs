using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class DoorBehaviour : MonoBehaviour, IRestartLevelElement
{
    [SerializeField] ActivationObject _activatorObject;
    [SerializeField] float _openSpeed;
    [SerializeField] Transform _openPosition;
    [SerializeField] Animator _anim;
    [SerializeField] AnimationClip _activateDoorClip;
    [SerializeField] SoundPlayer _verifiedSound;
    [SerializeField] SoundPlayer _doorUpSound;
    bool _isOpening = false;
    Vector2 _oPosition;

    [SerializeField] GameObject _doorParticlesPrefab;
    ParticleSystem _particleSystem;
    ParticleSystem.EmissionModule _particleSystemEmission;


    private void Awake()
    {
        _particleSystem = _doorParticlesPrefab.GetComponent<ParticleSystem>();
        _particleSystemEmission = _particleSystem.emission;
        _particleSystemEmission.enabled = false;
    }

    private void OnEnable()
    {
        _activatorObject.OnActivated += OnActivated;
    }

    private void OnDisable()
    {
        _activatorObject.OnActivated -= OnActivated;
    }
    void Start()
    {
        _oPosition = transform.position;
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
    }

    void Update()
    {
        if (_isOpening)
        {
            transform.position = Vector2.MoveTowards(transform.position, _openPosition.position, _openSpeed * Time.fixedDeltaTime);
            if (transform.position == _openPosition.position) FinishOpenDoor();
        }
    }

    private void FinishOpenDoor()
    {
        Debug.Log("Door opening finish");
        _particleSystemEmission.enabled = false;
        _isOpening = false;
    }

    private void OnActivated()
    {
        StartCoroutine(StartOpenDoor());
    }

    private IEnumerator StartOpenDoor()
    {
        Debug.Log("Door opening start");
        RoomCamManager.GetCameraManager().ShakeOnce(1f, 4);
        _verifiedSound.PlaySound();
        _anim.SetBool("activated", true);
        yield return new WaitForSeconds(0.8f);
        _doorUpSound.PlaySound();
        _particleSystemEmission.enabled = true;
        //InstantiateDoorParticles();
        yield return new WaitForSeconds(_activateDoorClip.length - 0.6f);
        _isOpening = true;
        _anim.SetBool("activated", true);
    }

    public void RestartLevel()
    {
        transform.position = _oPosition;
        _isOpening = false;
        UndoAnimation();
    }

    private void UndoAnimation()
    {
        _anim.Rebind();
        _anim.Update(0f);
    }

    private void InstantiateDoorParticles()
    {
        GameObject _particlesPref = Instantiate(_doorParticlesPrefab, transform.position, _doorParticlesPrefab.transform.rotation);
        ParticleSystem _particles = _particlesPref.GetComponent<ParticleSystem>();
        _particles.Play();
                
    }
}
