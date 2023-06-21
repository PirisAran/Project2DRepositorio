using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TecnocampusProjectII
{
    public class LevelController : MonoBehaviour
    {
        Transform _player;
        Collider2D _playerCollider;

        FireController _fire;

        [SerializeField]
        FinishLevelColliderBehaviour _collider;

        Thrower _playerThrower; 

        [SerializeField] Transform _umbra;
        public string m_NextLevel;

        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _umbraSpawnPoint;
        List<IRestartLevelElement> m_RestartLevelElements = new List<IRestartLevelElement>();

        private void Awake()
        {
        }

        private void SubscirbePlayer()
        {
            _player.GetComponent<PlayerController>().SubscribeToLvl(this);
        }

        private void SubscirbeFire()
        {
            _fire.SubscribeToLvl(this);
        }

        private void Start()
        {
            GameLogic.GetGameLogic().GetGameController().SetLevelController(this);
            _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
            _player.position = _playerSpawnPoint.position;
            _playerThrower = _player.GetComponent<Thrower>();
            _playerCollider = _player.GetComponent<Collider2D>();
            _fire = _player.GetComponentInChildren<FireController>();
            SubscirbePlayer();
            SubscirbeFire();


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
        }

        private bool ConditionNextLevel()
        {
            if (_collider != null)
            {
                return _collider.CanFinishLevel;
            }
            return false;
        }

        public void RestartLevel()
        {
            foreach (IRestartLevelElement l_RestartLevelElement in m_RestartLevelElements)
            {
                if (l_RestartLevelElement == null)
                {
                    m_RestartLevelElements.Remove(l_RestartLevelElement);
                }
                else
                    l_RestartLevelElement.RestartLevel();
            }
        }
        public void AddRestartLevelElement(IRestartLevelElement RestartLevelElement)
        {
            m_RestartLevelElements.Add(RestartLevelElement);
        }
        public void RemoveRestartLevelElement(IRestartLevelElement RestartLevelElement)
        {
            m_RestartLevelElements.Remove(RestartLevelElement);
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
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(_nextScene);
        }

        public void LoadNextScene()
        {
            SceneManager.LoadScene(m_NextLevel);
        }
    }
}