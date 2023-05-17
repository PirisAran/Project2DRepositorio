using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TecnocampusProjectII
{
	public class LevelController : MonoBehaviour
	{
        Transform _player;
        [SerializeField] Transform _umbra;
        public string m_NextLevel;
        
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _umbraSpawnPoint;

        public static LevelController Instance;
        private void Awake()
        {
        }

        private void Start()
        {
            Instance = this;
            _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
            RespawnPlayer();
        }

        private void Update()
        {
            if (ConditionNextLevel())
            {
                Debug.Log("load next level " + m_NextLevel);
                SceneManager.LoadSceneAsync(m_NextLevel);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                RespawnPlayer();
            }
        }

        private bool ConditionNextLevel()
        {
            return Input.GetKeyDown(KeyCode.N);
        }

        public void RespawnPlayer()
        {
            _player.position = _playerSpawnPoint.position;
            _umbra.position = _umbraSpawnPoint.position;
        }

        public void SetSpawnpoint(Vector3 playerPos, Vector3 umbraPos)
        {
            _playerSpawnPoint.position = playerPos;
            _umbraSpawnPoint.position = umbraPos;
        }
    }
}
