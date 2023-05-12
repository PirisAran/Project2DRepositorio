using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AplhaGameManager : MonoBehaviour
{

    [SerializeField]
    GameObject Umbra;
    [SerializeField]
    GameObject Player;

    [SerializeField] Transform plPos, umPos;
    // Start is called before the first frame update
    void Start()
    {
        Umbra.transform.position = umPos.position;
        Player.transform.position = plPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
