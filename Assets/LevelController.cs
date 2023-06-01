using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TecnocampusProjectII
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] Collider2D _childCollider;
        Transform _player;
        Collider2D _playerCollider;

        Thrower _playerThrower; 

        [SerializeField] Transform _umbra;
        public string m_NextLevel;

        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _umbraSpawnPoint;
        List<IRestartLevelElement> m_RestartLevelElements = new List<IRestartLevelElement>();

        private void Awake()
        {
        }

        private void Start()
        {
            GameLogic.GetGameLogic().GetGameController().SetLevelController(this);
            _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
            _player.position = _playerSpawnPoint.position;
            _playerThrower = _player.GetComponent<Thrower>();
            _playerCollider = _player.GetComponent<Collider2D>();


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
                StartCoroutine(ChangeScene(m_NextLevel));
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                RestartLevel();
            }
        }

        private bool ConditionNextLevel()
        {
            bool _passCondition;
            _passCondition = _childCollider.IsTouching(_playerCollider) && _playerThrower.HasFire;
            return _passCondition;
        }

        public void RestartLevel()
        {
            foreach (IRestartLevelElement l_RestartLevelElement in m_RestartLevelElements)
                l_RestartLevelElement.RestartLevel();
        }
        public void AddRestartLevelElement(IRestartLevelElement RestartLevelElement)
        {
            m_RestartLevelElements.Add(RestartLevelElement);
            RoomCamManager.GetCameraManager().StopShakeCamera();
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

        IEnumerator ChangeScene(string _nextScene)
        {
            //do transition scene
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene(_nextScene);
        }
    }
}