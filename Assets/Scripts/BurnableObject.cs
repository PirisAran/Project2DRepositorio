using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class BurnableObject : MonoBehaviour, IRestartLevelElement
{
    List<GameObject> _particlesCreated = new List<GameObject>();
    [SerializeField]
    float _timeToBurn = 3;

    SpriteRenderer _sr;
    Color _previousColor;
    [SerializeField] Color _desiredColor;

    Vector2 _hitPos;

    [SerializeField]
    GameObject _fireParticlesPrefab;

    IEnumerator _currentBurnRoutine;
    
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _previousColor = _sr.color;
    }

    private void Start()
    {
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _hitPos = collision.contacts[0].point;
        if (collision.gameObject.GetComponent<FireController>())
            Burn();
    }

    private Vector2 GetRandomPositionInCollider()
    {
        var collider = GetComponent<Collider2D>();
        Bounds bounds = collider.bounds;
        Vector2 randomPos = new Vector2(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y)
        );

        return randomPos;
    }

    void Burn()
    {
        var obj = Instantiate(_fireParticlesPrefab, _hitPos, Quaternion.identity);
        _particlesCreated.Add(obj);
        StartCoroutine(InstantiateMultipleFireParticlesInRandomPos(6));
        StartCoroutine(BurnAndDisable());
    }

    IEnumerator BurnAndDisable()
    {
        float timer = _timeToBurn;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            _sr.color = Color.Lerp(_previousColor, _desiredColor, Mathf.Clamp01((_timeToBurn - timer) / _timeToBurn));
            yield return null;
        }

        EndBurn();
    }

    IEnumerator InstantiateMultipleFireParticlesInRandomPos(int num)
    {
        for (int i = 0; i < num; i++)
        {
            yield return new WaitForSeconds(_timeToBurn/num);
            var obj  = Instantiate(_fireParticlesPrefab, GetRandomPositionInCollider(), Quaternion.identity);
            _particlesCreated.Add(obj);
        }
    }

    private void EndBurn()
    {
        gameObject.SetActive(false);
        DestroyAllParticlesCreated();

    }

    private void DestroyAllParticlesCreated()
    {
        foreach (GameObject particles in _particlesCreated)
        {
            if (particles != null)
            {
                Destroy(particles);
            }
        }
    }

    public void RestartLevel()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        _sr.color = _previousColor;
        DestroyAllParticlesCreated();
        _particlesCreated = new List<GameObject>();
    }
}
