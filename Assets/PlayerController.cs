using UnityEngine;

namespace TecnocampusProjectII
{
	public class PlayerController : MonoBehaviour, IRestartLevelElement
	{
		private void Start()
		{
			GameLogic l_GameLogic=GameLogic.GetGameLogic();
			if(l_GameLogic.GetGameController().m_Player==this)
				DontDestroyOnLoad(this.gameObject);
			else
				Destroy(gameObject);
			l_GameLogic.GetGameController().GetLevelController().AddRestartLevelElement(this);
		}

        public void RestartLevel()
        {
			GameLogic l_GameLogic = GameLogic.GetGameLogic();
			transform.position = l_GameLogic.GetGameController().GetLevelController().GetPlayerSpawnPoint().position;
        }
	}
}
