using System;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class BossMusicManager : MonoBehaviour
{
    [SerializeField] AudioSource _m1, _m2, _m3;

    PlayerController _player;

    bool _m2Played = false;

    private void Awake()
    {
        DeactivateAllMusic();
        _m1.gameObject.SetActive(true);
    }

    private void DeactivateAllMusic()
    {
        _m1.gameObject.SetActive(false);
        _m2.gameObject.SetActive(false);
        _m3.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_m2Played && !_m1.isPlaying)
        {
            DeactivateAllMusic();
            _m2.gameObject.SetActive(true);
            _m2Played = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TRIGGER MUSIC");
        if (collision.transform == _player.transform)
        {
            DeactivateAllMusic();
            _m3.gameObject.SetActive(true);
        }
    }
}
