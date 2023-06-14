using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class TutorialManager : MonoBehaviour, IRestartLevelElement
{
    FireController _fire;
    Transform _player;
    Thrower _thrower;

    bool firstTitme = true;

    [SerializeField] Transform firePos;

    public void RestartLevel()
    {
        firstTitme = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
        _fire = _player.GetComponentInChildren<FireController>();
        _thrower = _player.GetComponent<Thrower>();
    }

    // Update is called once per frame
    void Update()
    {
        if (firstTitme)
        {
            _thrower.SetAttachFireToBody(false);
            _fire.transform.position = firePos.position;
            firstTitme = false;
        }
    }
}
