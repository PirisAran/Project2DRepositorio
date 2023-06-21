using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenuCanva;

    public static bool _isPaused;
    float _continueTransitionTime = 1f;
    float _currentTime;


    void Start()
    {
        _pauseMenuCanva.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        StartCoroutine(ResumeGameCoroutine());
    }

    public void PauseGame()
    {
        _pauseMenuCanva.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    
    IEnumerator ResumeGameCoroutine()
    {
        _isPaused = false;
        _pauseMenuCanva.SetActive(false);

        float timeToResume = _continueTransitionTime;
        float timer = 0;
        while (timer < timeToResume)
        {
            timer += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp01(timer / timeToResume);
            yield return null;
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuGold");
        _isPaused = false;
    }

}
