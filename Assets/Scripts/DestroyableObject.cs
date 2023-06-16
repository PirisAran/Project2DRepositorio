using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class DestroyableObject : MonoBehaviour, IRestartLevelElement
{
    [SerializeField] Color _particleColor;
    [SerializeField] GameObject _particlesPrefab;
    [SerializeField] Vector2 _particlesPosition;
    GameObject _particlesCreated;

    [SerializeField] GameObject _objectToDestroy;
    bool _activeOnAwake;
    [SerializeField] private float _timeToDestroy = 0.5f;
    float _chanceToSpawnParticles = 0.3f;
    float _chanceToShakeCamera = 0.3f;

    public void RestartLevel()
    {
        StopAllCoroutines();
        _objectToDestroy.SetActive(_activeOnAwake);
        Destroy(_particlesCreated);
        _particlesCreated = null;
    }
    private void Awake()
    {
        if (_objectToDestroy == null) _objectToDestroy = gameObject;
        _activeOnAwake = _objectToDestroy.active;

        var collider2D = GetComponentInChildren<Collider2D>();

        if (collider2D != null) _particlesPosition = collider2D.transform.position;
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
        RandomInstantiateParticles();
        RandomShakeCamera();
    }

    private void InstantiateParticles()
    {
        _particlesCreated = Instantiate(_particlesPrefab, _particlesPosition, Quaternion.identity, null);
        SetParticleColor(_particleColor, _particlesCreated);
    }

    private void SetParticleColor(Color particleColor, GameObject particlesCreated)
    {
        ParticleSystem.MainModule main = particlesCreated.GetComponentInChildren<ParticleSystem>().main;
        main.startColor = particleColor;
    }

    private void RandomInstantiateParticles()
    {
        float randomValue = Random.value;

        if (randomValue <= _chanceToSpawnParticles)
        {
            InstantiateParticles();
        }
    }

    private void RandomShakeCamera()
    {
        float randomValue = Random.value;
        if (randomValue <= _chanceToShakeCamera)
        {
            RoomCamManager.GetCameraManager().ShakeOnce(1, 1);
        }
    }
}
