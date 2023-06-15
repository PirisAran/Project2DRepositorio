using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;
public class ParticlePrefabBehaviour : MonoBehaviour, IRestartLevelElement
{
    [SerializeField] float _lifeTime;
    bool _destroyed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
        StartCoroutine(DestroyAfterTime(_lifeTime));
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        _destroyed = true;
        Destroy(gameObject);
    }

    public void RestartLevel()
    {
        if (!_destroyed)
        Destroy(gameObject);
    }
}
