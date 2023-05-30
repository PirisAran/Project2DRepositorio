using System;
using System.Collections.Generic;
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
        List<IRestartLevelElement> m_RestartLevelElements = new List<IRestartLevelElement>();

        private void Awake()
        {
            GameLogic.GetGameLogic().GetGameController().SetLevelController(this);
        }

        private void Start()
        {
            _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
            _player.position = _playerSpawnPoint.position;
            if (_umbra != null)
            {
                _umbra.position = _umbraSpawnPoint.position;
            }   
        }

        private void Update()
        {
            if (ConditionNextLevel())
            {
                Debug.Log("load next level " + m_NextLevel);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                RestartLevel();
            }
        }

        private bool ConditionNextLevel()
        {
            return Input.GetKeyDown(KeyCode.N);
        }

        public void RestartLevel()
        {
            foreach (IRestartLevelElement l_RestartLevelElement in m_RestartLevelElements)
                l_RestartLevelElement.RestartLevel();
        }
        public void AddRestartLevelElement(IRestartLevelElement RestartLevelElement)
        {
            m_RestartLevelElements.Add(RestartLevelElement);
        }

        public void SetSpawnPoint(Vector3 playerPos, Vector3 umbraPos)
        {
            _playerSpawnPoint.position = playerPos;
            _umbraSpawnPoint.position = umbraPos;
        }
        public Transform GetPlayerSpawnPoint()
        {
            return _playerSpawnPoint;
        }
        public Transform GetUmbraSpawnPoint()
        {
            return _umbraSpawnPoint;
        }
    }
}