using UnityEngine;

namespace TecnocampusProjectII
{
	public class GameLogic : MonoBehaviour
	{
		static GameLogic m_GameLogic;
		GameLogicData m_GameLogicData;
		bool m_GameStarted=false;
		GameController m_GameController;

		static public GameLogic GetGameLogic()
		{
			if(m_GameLogic==null)
				m_GameLogic=InitGameLogic();
			return m_GameLogic;
		}
		static GameLogic InitGameLogic()
		{
			GameObject l_GameObject=new GameObject("GameLogic");
			l_GameObject.transform.position=Vector3.zero;
			GameLogic.DontDestroyOnLoad(l_GameObject);
			return l_GameObject.AddComponent<GameLogic>();
		}
		private void Start()
		{
			m_GameLogicData = Resources.Load<GameLogicData>("GameLogicData");
		}
		public void SetGameStarted(bool GameStarted)
		{
			m_GameStarted=GameStarted;
		}
		public bool IsGameStarted()
		{
			return m_GameStarted;
		}
		public void SetGameController(GameController _GameController)
		{
			m_GameController=_GameController;
		}
		public GameController GetGameController()
		{
			return m_GameController;
		}
		public GameLogicData GetGameLogicData()
		{
			return m_GameLogicData;
		}

	}
}
