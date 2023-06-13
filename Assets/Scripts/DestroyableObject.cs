using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class DestroyableObject : MonoBehaviour, IRestartLevelElement
{
    [SerializeField] Color _particleColor;
    [SerializeField] GameObject _particlesPrefab;
    GameObject _particlesCreated;

    [SerializeField] GameObject _objectToDestroy;
    bool _activeOnAwake;
    [SerializeField] private float _timeToDestroy = 0.5f;

    public void RestartLevel()
    {
        _objectToDestroy.SetActive(_activeOnAwake);
        Destroy(_particlesCreated);
        _particlesCreated = null;
    }
    private void Awake()
    {
        if (_objectToDestroy == null) _objectToDestroy = gameObject;
        _activeOnAwake = _objectToDestroy.active;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
    }

    public void StartDestroyObject()
    {
        StartCoroutine(DestroyObjectCoroutine(_timeToDestroy));
    }

    private IEnumerator DestroyObjectCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        _objectToDestroy.SetActive(false);
        InstantiateParticles();
    }

    private void InstantiateParticles()
    {
        _particlesCreated = Instantiate(_particlesPrefab, transform.position, Quaternion.identity);
        //particles.
    }
}
