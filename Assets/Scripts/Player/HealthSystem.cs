using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;
using System;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] GameObject _killParticles;
    ParticleSystem _particleSystem;
    Transform _player;

    [SerializeField] GameObject _ignisParts;

    private void Awake()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
        _particleSystem = _killParticles.GetComponent<ParticleSystem>();
    }
    public void KillPlayer()
    {
        StartCoroutine(KillOnEndFrame());
        
    }

    private IEnumerator KillOnEndFrame()
    {
        InstantiateParticles();
        _ignisParts.SetActive(false);
        yield return new WaitForSeconds(0.7f);

        yield return null;
        var l_gameLogic = GameLogic.GetGameLogic();
        l_gameLogic.GetGameController().GetLevelController().RestartLevel();
    }

    private void InstantiateParticles()
    {
        GameObject particleObj = Instantiate(_killParticles, _player.position, _killParticles.transform.rotation);

        ParticleSystem instantiateParticleSystem = particleObj.GetComponent<ParticleSystem>();

        instantiateParticleSystem.Play();
    }
}
