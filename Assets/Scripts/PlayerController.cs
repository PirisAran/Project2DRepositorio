using System;
using UnityEngine;

namespace TecnocampusProjectII
{
	public class PlayerController : MonoBehaviour, IRestartLevelElement
	{
		LevelController currentLvlController;
		[SerializeField] GameObject _ignisParts;
		Rigidbody2D _rb;
		private void Start()
		{
			GameLogic l_GameLogic=GameLogic.GetGameLogic();
			if(l_GameLogic.GetGameController().m_Player==this)
				DontDestroyOnLoad(this.gameObject);
			else
				Destroy(gameObject);
		}

        public void RestartLevel()
        {
			GameLogic l_GameLogic = GameLogic.GetGameLogic();
			transform.position = currentLvlController.GetPlayerSpawnPoint().position;
			_ignisParts.SetActive(true);
		}

        internal void SubscribeToLvl(LevelController levelController)
        {
			levelController.AddRestartLevelElement(this);
			currentLvlController = levelController;
		}
    }
}
