using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoomFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject virtualCamera;

    bool _playerInTrigger = false;



    //private void Update()
    //{
    //    if (_playerInTrigger)
    //    {
    //        virtualCamera.SetActive(true);
    //    }
    //    else
    //    {
    //        virtualCamera.SetActive(false);
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        _playerInTrigger = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        _playerInTrigger = false;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.SetActive(true);
        }

    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            virtualCamera.SetActive(false);

        }
    }
}