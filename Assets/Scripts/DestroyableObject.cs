using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class DestroyableObject : MonoBehaviour, IRestartLevelElement
{
    [SerializeField] GameObject _objectToDestroy;
    public void RestartLevel()
    {
        _objectToDestroy.SetActive(true);
    }
    private void Awake()
    {
        if (_objectToDestroy == null) _objectToDestroy = gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
    }

    public void StartDestroyObject()
    {
        _objectToDestroy.SetActive(false);
    }
}
