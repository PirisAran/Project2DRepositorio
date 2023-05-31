using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class BurnableObject: MonoBehaviour, IRestartLevelElement
{
    [SerializeField]
    float _timeToBurn = 3;

    SpriteRenderer _sr;
    Color _previousColor;

    [SerializeField]
    GameObject _fireParticlesPrefab;
    
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _previousColor = _sr.color;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<FireController>())
            Burn();
    }

    void Burn()
    {
        Instantiate(_fireParticlesPrefab, transform.position, Quaternion.identity);
        StartCoroutine(BurnAndDisable());
    }

    IEnumerator BurnAndDisable()
    {
        Color desiredColor = Color.black;
        float timer = _timeToBurn;
        while (timer > 0)
        {
            timer += Time.deltaTime;
            Color.Lerp(_previousColor, desiredColor, Mathf.Clamp01(timer / _timeToBurn));
            yield return null;
        }

        EndBurn();
    }

    private void EndBurn()
    {
        gameObject.SetActive(false);
    }

    public void RestartLevel()
    {
        gameObject.SetActive(true);
        _sr.color = _previousColor;
    }
}
