using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class GroupOfUmbrasBehaviour : MonoBehaviour
{
    [SerializeField] List<UmbraMini> _umbraMiniList = new List<UmbraMini>();
    [SerializeField] SoundPlayer _huntingSound;

    [SerializeField] float _umbrasSpeed;
    PlayerController _player;
    FireController _fire;
    public bool _activated = false;

    private void OnEnable()
    {
        FireController.OnFireDestroyed += ActivateUmbras;
    }

    private void OnDisable()
    {
        FireController.OnFireDestroyed = ActivateUmbras;
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player;
        _fire = _player.GetComponentInChildren<FireController>();
    }

    private void ActivateUmbras()
    {
        if (_activated)
        {
            return;
        }
        _activated = true;
        _huntingSound.PlaySound();  
        foreach (var umbra in _umbraMiniList)
        {
            umbra.Activate(_umbrasSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ActivateIfNoLight();
        transform.position = _player.transform.position;
    }

    private void ActivateIfNoLight()
    {
        if (_fire.LightRange <= 0)
        {
            ActivateUmbras();
        }
    }
}
