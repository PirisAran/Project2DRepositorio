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

		[SerializeField] SoundPlayer _wooshSound;

		[SerializeField] GameObject _mainMenuCanva;
		[SerializeField] GameObject _optionsCanva;

		[SerializeField] ParticleSystem _parentFuego;
		[SerializeField] ParticleSystem _childFuego;

		ParticleSystem.ColorOverLifetimeModule _parentColorModule;
		ParticleSystem.ColorOverLifetimeModule _childColorModule;

		[SerializeField]
		Gradient _oGradient, _hardGradient, _easyGradient;

		void Start()
		{
			_parentColorModule = _parentFuego.colorOverLifetime;
			_childColorModule = _childFuego.colorOverLifetime;
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
        public void OnStartClicked()
		{
			Debug.Log("START");
			SceneManager.LoadSceneAsync(_firstLevelScene);
		}

		public void OnOptionsClicked()
        {
			DisableAllCanvas();
			_optionsCanva.SetActive(true);
        }

		public void OnReturnClicked()
        {
			DisableAllCanvas();
			_mainMenuCanva.SetActive(true);
			SetFireColor(_oGradient);
        }

        private void DisableAllCanvas()
        {
			_mainMenuCanva.SetActive(false);
			_optionsCanva.SetActive(false);
		}

		public void OnEasyDifficultyClicked()
        {
			DifficultyManager.SetDifficultyLevel(DifficultyManager.DifficultyLevels.Easy);
			ChangeFireColor(DifficultyManager._currentDifficultyLevel);
        }

		public void OnHardDifficultyClicked()
		{
			DifficultyManager.SetDifficultyLevel(DifficultyManager.DifficultyLevels.Hard);
			ChangeFireColor(DifficultyManager._currentDifficultyLevel);
		}

		private void ChangeFireColor(DifficultyManager.DifficultyLevels d)
        {
            switch (d)
            {
                case DifficultyManager.DifficultyLevels.Easy:
					SetFireColor(_easyGradient);
					break;
                case DifficultyManager.DifficultyLevels.Normal:
                    break;
                case DifficultyManager.DifficultyLevels.Hard:
					SetFireColor(_hardGradient);
                    break;
				default:
                    break;
            }

			_wooshSound.PlaySound();
        }

		private void SetFireColor(Gradient _gradient)
        {
			_parentColorModule.color = _gradient;
			_childColorModule.color = _gradient;
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
