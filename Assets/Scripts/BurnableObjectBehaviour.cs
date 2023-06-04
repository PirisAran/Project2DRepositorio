using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class BurnableObjectBehaviour : MonoBehaviour, IRestartLevelElement
{
    [Header("Scripts Utilizados")]
    [SerializeField] Spawner _spawner;

    Queue<GameObject> _particlesCreated = new Queue<GameObject>();
    [SerializeField]
    float _timeToBurn = 3;

    [SerializeField]
    SpriteRenderer _sr;
    Color _previousColor;
    [SerializeField] Color _desiredColor;

    Vector2 _hitPos;
    Quaternion _particlesRot = Quaternion.Euler(-90, 0, 0);

    
    private void Awake()
    {
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
        var obj = _spawner.SpawnOne(_hitPos, _particlesRot);
        _particlesCreated.Enqueue(obj);
        StartCoroutine(InstantiateMultipleFireParticlesInRandomPos(9));
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
        StartCoroutine(EndBurn());
    }

    IEnumerator InstantiateMultipleFireParticlesInRandomPos(int num)
    {
        for (int i = 0; i < num; i++)
        {
            yield return new WaitForSeconds(_timeToBurn/num);
            var obj  = _spawner.SpawnOne(GetRandomPositionInCollider(), _particlesRot);
            _particlesCreated.Enqueue(obj);
        }
    }

    private IEnumerator EndBurn()
    {
        var noAlphaColor = new Color(_desiredColor.r, _desiredColor.g, _desiredColor.b, 0);
        float disapearTimer = _timeToBurn/1.5f;
        float timer = disapearTimer;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            _sr.color = Color.Lerp(_desiredColor, noAlphaColor, Mathf.Clamp01((_timeToBurn - timer) / disapearTimer));
            yield return null;
        }
        gameObject.SetActive(false);
        DestroyAllParticlesCreated();

    }

    private void DestroyAllParticlesCreated()
    {
        for (int i = 0; i < _particlesCreated.Count; i++)
        {
            var obj = _particlesCreated.Dequeue();
            Destroy(obj);
        }
    }

    public void RestartLevel()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        _sr.color = _previousColor;
        DestroyAllParticlesCreated();
        _particlesCreated = new Queue<GameObject>();
    }
}
