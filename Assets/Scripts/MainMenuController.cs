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
		[SerializeField] string _LVLSELECTOR;

		[SerializeField] GameObject _mainMenuCanva;
		[SerializeField] GameObject _optionsMenuCanva;

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



			DisableAllCanvas();
			_mainMenuCanva.SetActive(true);
		}

        private void Update()
        {
			if (Input.GetKeyDown(KeyCode.Space))
            {
				SceneManager.LoadSceneAsync(_firstLevelScene);
			}
        }

        // I ENUMERATOR CON ANIMACIONES UJUJU gold
        public void OnStartClicked()
		{
			Debug.Log("START");
			SceneManager.LoadSceneAsync(_firstLevelScene);
		}
		public void OnOptionsClicked()
		{
			Debug.Log("OPTIONS");
			DisableAllCanvas();
			_optionsMenuCanva.SetActive(true);
		}

		public void OnVolumeClicked()
        {
			Debug.Log("VOLUME");
        }

		public void OnLevelSelectorClicked()
		{
			SceneManager.LoadSceneAsync(_LVLSELECTOR);
		}

		public void OnReturnClicked()
        {
			DisableAllCanvas();
			_mainMenuCanva.SetActive(true);
			Debug.Log("RETURN");
		}

		private void DisableAllCanvas()
        {
			_optionsMenuCanva.SetActive(false);
			_mainMenuCanva.SetActive(false);
		}

        public void OnExitClicked()
		{
			Application.Quit();
		}
	}
}
