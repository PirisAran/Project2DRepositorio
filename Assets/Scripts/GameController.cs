using UnityEngine;

namespace TecnocampusProjectII
{
	public class GameController : MonoBehaviour
	{
		public PlayerController m_Player;
		LevelController m_CurrentLevelController;
		public void Awake()
		{
			GameLogic l_GameLogic=GameLogic.GetGameLogic();
			if(l_GameLogic.GetGameController()==null)
			{
				l_GameLogic.SetGameController(this);
				DontDestroyOnLoad(this.gameObject);
			}
			else
				Destroy(gameObject);
		}
		public void SetLevelController(LevelController _LevelController)
		{
			m_CurrentLevelController = _LevelController;
		}
		public LevelController GetLevelController()
		{
			return m_CurrentLevelController;
		}

	}
}
