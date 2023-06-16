using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;
using System;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] GameObject _deathParticles;
    ParticleSystem _particleSystem;
    Transform _player;

    [SerializeField] SoundPlayer _deathSound;

    [SerializeField] GameObject _ignisParts;

    Rigidbody2D _rb;

    bool _doingCoroutine = false;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
        _particleSystem = _deathParticles.GetComponent<ParticleSystem>();
    }
    public void KillPlayer()
    {
        if (_doingCoroutine)
        {
            return;
        }
        StartCoroutine(KillOnEndFrame());
    }

    private IEnumerator KillOnEndFrame()
    {
        _doingCoroutine = true;
        _rb.bodyType = RigidbodyType2D.Static;
        _deathSound.PlaySound();
        //DoParticleEffect();
        _ignisParts.SetActive(false);
        DeathTransitionBehaviour.DoDeathTransition();
        yield return new WaitForSeconds(0.5f);
        var l_gameLogic = GameLogic.GetGameLogic();
        l_gameLogic.GetGameController().GetLevelController().RestartLevel();
        DeathTransitionBehaviour.UndoDeathTransition();
        yield return null;
        _doingCoroutine = false;
    }

    private void DoParticleEffect()
    {
        _particleSystem.Emit(40);
    }
}
