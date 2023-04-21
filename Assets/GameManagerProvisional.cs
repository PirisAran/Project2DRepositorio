using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerProvisional : MonoBehaviour
{
    [SerializeField]
    Fire Fire;
    [SerializeField]
    Transform Player;
    [SerializeField]
    Transform FireInitPos;
    [SerializeField]
    Transform PlayerInitPos;
    [SerializeField]
    GameObject Umbra;
    [SerializeField]
    FireThrower FireThrower;

    private void Start()
    {
        Umbra.SetActive(false);
        Fire.DetachFromPlayer();
        Fire.GetComponent<Collider2D>().isTrigger = false;
        Fire.transform.position = FireInitPos.position;
        Player.position = PlayerInitPos.position;

    }


    private void OnEnable()
    {
        FireThrower.OnFirePickedUp += OnFirePickedUp;
    }
    private void OnDisable()
    {
        FireThrower.OnFirePickedUp -= OnFirePickedUp;
    }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnFirePickedUp()
    {
        Umbra.SetActive(true);
        Debug.Log("umbra activao");
    }
}
