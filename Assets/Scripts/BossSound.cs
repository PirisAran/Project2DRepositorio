using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSound : MonoBehaviour
{
    [SerializeField] SoundPlayer _bossSound;

    private void Start()
    {
        _bossSound.PlaySound();
    }
}
