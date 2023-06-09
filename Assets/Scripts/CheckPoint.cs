using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;
using UnityEngine.Rendering.Universal;

public class CheckPoint : PlayerWithFireActivation
{
    [SerializeField] Transform _playerSpawnPoint;
    [SerializeField] Transform _umbraSpawnPoint;
    [SerializeField] Animator _anim;
    [SerializeField] protected Light2D _light;
    [SerializeField] protected Color _innactiveLightColor, _activeLightColor;

    public static Action OnCheckPointActivated;

    [SerializeField] SoundPlayer _checkPointSound;
    [SerializeField] GameObject _checkPointParticlePrefab;

    private void Awake()
    {
        _light.color = _innactiveLightColor;
    }
    protected override void DoAnimation()
    {
        _anim.SetBool("startActivation", true);
        StartCoroutine(DoAnimationTimeLater(1f));
    }

    protected override void Activate()
    {
        base.Activate();
        OnCheckPointActivated?.Invoke();
        GameLogic l_GameLogic = GameLogic.GetGameLogic();
        l_GameLogic.GetGameController().GetLevelController().SetSpawnPoint(_playerSpawnPoint.position, _umbraSpawnPoint.position);
        Debug.Log("checkpoint activated");
        _checkPointSound.PlaySound();

        InstantiateParticles();
    }

    IEnumerator DoAnimationTimeLater(float time)
    {
        yield return new WaitForSeconds(time);
        _light.color = _activeLightColor;
        _anim.SetBool("activated", true);
    }

    private void InstantiateParticles()
    {
        GameObject particleObj = Instantiate(_checkPointParticlePrefab, transform.position, _checkPointParticlePrefab.transform.rotation);

        ParticleSystem instantiateParticleSystem = particleObj.GetComponent<ParticleSystem>();

        instantiateParticleSystem.Play();
    }
}
