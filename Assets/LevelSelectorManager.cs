using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectorManager : MonoBehaviour
{
    [SerializeField] string tutorial, floor1, floor2, boss, mainmenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTutorialClicked()
    {
        SceneManager.LoadScene(tutorial);
    }
    public void OnCaveClicked()
    {
        SceneManager.LoadScene(floor1);
    }
    public void OnTempleClicked()
    {
        SceneManager.LoadScene(floor2);
    }
    public void OnBossClicked()
    {
        SceneManager.LoadScene(boss);
    }
    public void OnReturnClicked()
    {
        SceneManager.LoadScene(mainmenu);
    }
}
