using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;
public class ParticlePrefabBehaviour : MonoBehaviour, IRestartLevelElement
{
    [SerializeField] float _lifeTime;
    bool _destroyed = false;

    static float coroutinescount = 0;
    static float restartcount = 0;

    LevelController _lvlController;

    // Start is called before the first frame update
    void Start()
    {
        _lvlController = GameLogic.GetGameLogic().GetGameController().GetLevelController();
        _lvlController.AddRestartLevelElement(this);
        StartCoroutine(DestroyAfterTime(_lifeTime));
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        _destroyed = true;
        Debug.Log(gameObject.name);
        gameObject.SetActive(false);
    }

    public void RestartLevel()
    {
    }
}
