using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class DestroyableObject : MonoBehaviour, IRestartLevelElement
{
    public void RestartLevel()
    {
        gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDestroyObject()
    {
        gameObject.SetActive(false);
    }
}
