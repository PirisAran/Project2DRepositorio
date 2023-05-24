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
    [SerializeField] Light2D _light;
    [SerializeField] Color _innactiveLightColor, _activeLightColor;
    [SerializeField] Sprite _noLeafsSprite;
    [SerializeField] SpriteRenderer _spriteRenderer;

    public static Action OnCheckPointActivated;

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
        LevelController.Instance.SetSpawnpoint(_playerSpawnPoint.position, _umbraSpawnPoint.position);
        Debug.Log("checkpoint activated");
    }

    IEnumerator DoAnimationTimeLater(float time)
    {
        yield return new WaitForSeconds(time);
        _light.color = _activeLightColor;
        _anim.SetBool("activated", true);
    }

}
