using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CinematicController : MonoBehaviour
{
    [SerializeField] string _nextScene;
    [SerializeField] VideoPlayer _videoPlayer;
    [SerializeField] private KeyCode _skipKey = KeyCode.Space;

    // Start is called before the first frame update
    void Start()
    {
        GameLogic l_GameLogic = GameLogic.GetGameLogic();
        if (l_GameLogic.GetGameController() != null)
        {
            GameObject.Destroy(l_GameLogic.GetGameController().m_Player.gameObject);
            GameLogic.GetGameLogic().SetGameController(null);
        }
    }

    private void OnEnable()
    {
        if (_videoPlayer == null) return;
        _videoPlayer.loopPointReached += LoadNextScene;
    }

    private void OnDisable()
    {
        if (_videoPlayer == null) return;
        _videoPlayer.loopPointReached -= LoadNextScene;
    }

    private void Update()
    {
        if (Input.GetKey(_skipKey))
        {
            SceneManager.LoadScene(_nextScene);
        }
    }

    public void LoadNextScene(VideoPlayer vp)
    {
        SceneManager.LoadScene(_nextScene);
    }
}
