using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;
using System;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] GameObject _deathParticles;
    ParticleSystem _particleSystem;

    FireController _fire;

    [SerializeField] SoundPlayer _deathSound;

    [SerializeField] GameObject _ignisParts;

    Rigidbody2D _rb;

    bool _doingCoroutine = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _fire = GameLogic.GetGameLogic().GetGameController().m_Player.GetComponentInChildren<FireController>();
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

        //hago q la cam no tenga blend
        var cam = Camera.main;
        var brainCam = cam.GetComponent<Cinemachine.CinemachineBrain>();
        brainCam.m_DefaultBlend.m_Style = Cinemachine.CinemachineBlendDefinition.Style.Cut;

        //fire cae, desaparece ignis
        _fire.BeThrown(Vector2.right, 0);
        _rb.bodyType = RigidbodyType2D.Static;
        _ignisParts.SetActive(false);

        //effects
        _deathSound.PlaySound();
        DoParticleEffect();

        //transition in
        DeathTransitionBehaviour.DoDeathTransition(4);
        yield return new WaitForSeconds(2.5f);

        //black screen on
        var blackScreen = cam.GetComponent<BlackScreen>();
        blackScreen.SetActiveBlackScreen(true);

        //reset
        var l_gameLogic = GameLogic.GetGameLogic();
        l_gameLogic.GetGameController().GetLevelController().RestartLevel();

        yield return new WaitForSeconds(0.1f);

        //blakcscreen off
        blackScreen.SetActiveBlackScreen(false);

        //transition off
        DeathTransitionBehaviour.UndoDeathTransition(12);


        yield return new WaitForSeconds(1);
        brainCam.m_DefaultBlend.m_Style = Cinemachine.CinemachineBlendDefinition.Style.EaseInOut;
        _doingCoroutine = false;

    }

    private void DoParticleEffect()
    {
        _particleSystem.Emit(40);
    }
}
