using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;
public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_player.position.x, _player.position.y, -10);
    }
}
