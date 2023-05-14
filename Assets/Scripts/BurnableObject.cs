using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableObject: MonoBehaviour
{
    [SerializeField]
    float TimeToBurn = 3;

    [SerializeField]
    GameObject Particles;

    float _timer;

    private void Awake()
    {
        Particles.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<FireController>())
            StartToBurn();
    }

    void StartToBurn()
    {
        StartCoroutine(StartCountDown());
        Particles.SetActive(true);
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
        Particles.SetActive(false);
        Destroy(gameObject);
        Debug.Log("BURNED OBJECT:" + gameObject.name);
    }
}
