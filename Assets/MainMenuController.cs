using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TecnocampusProjectII
{
	public class MainMenuController : MonoBehaviour
	{
		void Start()
		{
			GameLogic l_GameLogic=GameLogic.GetGameLogic();
			if(l_GameLogic.GetGameController()!=null)
			{
				GameObject.Destroy(l_GameLogic.GetGameController().m_Player.gameObject);
				GameObject.Destroy(l_GameLogic.GetGameController().gameObject);
				GameLogic.GetGameLogic().SetGameController(null);
			}
			l_GameLogic.SetGameStarted(true);
		}
		public void OnStartClicked()
		{
			SceneManager.LoadSceneAsync("Level1Scene");
		}
	}
}
