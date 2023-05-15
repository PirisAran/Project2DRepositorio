using UnityEngine;

namespace TecnocampusProjectII
{
	public class GameController : MonoBehaviour
	{
		public PlayerController m_Player;

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
	}
}
