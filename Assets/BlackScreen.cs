using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreen : MonoBehaviour
{
    [SerializeField] Camera _blackScreenCamera;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _blackScreenCamera.transform.position = transform.position;
    }

    public void SetActiveBlackScreen(bool v)
    {
        _blackScreenCamera.gameObject.SetActive(v);
    }
}
