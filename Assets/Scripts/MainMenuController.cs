using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TecnocampusProjectII
{

	public class MainMenuController : MonoBehaviour
	{
		[SerializeField] string _firstLevelScene;

		[SerializeField] GameObject _mainMenuCanva;

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
			Debug.Log("START");
			SceneManager.LoadSceneAsync(_firstLevelScene);
		}

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
				SceneManager.LoadSceneAsync("MenuGold");
				Debug.Log("reload");
			}
		}

        public void OnExitClicked()
		{
			Application.Quit();
		}
	}
}
