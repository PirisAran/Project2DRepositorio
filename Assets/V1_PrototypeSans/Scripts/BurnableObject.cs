using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableObject: MonoBehaviour
{
    [SerializeField]
    float TimeToBurn = 3;

    float _timer;

    private void Awake()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("tOUCHED");
        if (collision.gameObject.GetComponent<Fire>())
        {
            StartToBurn();
        }
    }

    void StartToBurn()
    {
        StartCoroutine(StartCountDown());
    }

    IEnumerator StartCountDown()
    {
        _timer = 0;
        while (_timer < TimeToBurn)
        {
            _timer += Time.deltaTime;
            yield return null;
        }
        EndBurn();
    }

    private void EndBurn()
    {
        Destroy(gameObject);
        Debug.Log("BURNED OBJECT:" + gameObject.name);
    }
}
