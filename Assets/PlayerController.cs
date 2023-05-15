using UnityEngine;

namespace TecnocampusProjectII
{
	public class PlayerController : MonoBehaviour
	{
		private void Start()
		{
			GameLogic l_GameLogic=GameLogic.GetGameLogic();
			if(l_GameLogic.GetGameController().m_Player==this)
				DontDestroyOnLoad(this.gameObject);
			else
				Destroy(gameObject);
		}
	}
}
