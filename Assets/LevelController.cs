using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TecnocampusProjectII
{
	public class LevelController : MonoBehaviour
	{
        [SerializeField] GameObject _player;
		public string m_NextLevel;
        
        [SerializeField] private Transform _spawnPoint;
        private void Awake()
        {
            _spawnPoint.position = Vector2.zero;
            _player.transform.position = _spawnPoint.position;
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
            _player.transform.position = _spawnPoint.position;
        }

        public void SetSpawnpoint(Vector3 pos)
        {
            _spawnPoint.position = pos;
        }
    }
}
