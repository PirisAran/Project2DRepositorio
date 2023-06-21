using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TecnocampusProjectII;
public class StalactitaTrapBehaviour : MonoBehaviour, IRestartLevelElement
{
    [Header("Scripts Utilizados")]
    [SerializeField] PlayerKiller _playerKiller;
    [SerializeField] PlayerDetector _playerDetector;
    [SerializeField] FireDestroyer _fireDestroyer;

    bool _playerWasHit;

    Vector2 _oPosition;
    Rigidbody2D _rb;

    [SerializeField] GameObject _particleStalactitaPrefab;
    [SerializeField] Transform _particleSpawnPosition;

    [SerializeField] GameObject _particleStalactitaStartPrefab;
    [SerializeField] Transform _particleSpawnStartPosition;

    ParticleSystem _particleSystem1;
    ParticleSystem _particleSystem2;

    [SerializeField] SoundPlayer _fallingSound;

    [SerializeField]
    private Animator _animator;
    private bool _isPlayingAnimation = false;

    private bool _triggerExitRoof = false;

    private void OnEnable()
    {
        _playerDetector.OnPlayerDetected += OnPlayerDetected;
    }

    private void OnDisable()
    {
        _playerDetector.OnPlayerDetected -= OnPlayerDetected;
    }

    private void Awake()
    {
        _particleSystem1 = _particleStalactitaPrefab.GetComponent<ParticleSystem>();
        _particleSystem2 = _particleStalactitaStartPrefab.GetComponentInChildren<ParticleSystem>();
        _rb = GetComponent<Rigidbody2D>();
        Init();
    }

    private void Init()
    {
        _rb.bodyType = RigidbodyType2D.Static;
        _playerKiller.SetCanKill(false);
        _fireDestroyer.SetCanDestroy(false);
        _oPosition = transform.position;
        GetComponent<Collider2D>().isTrigger = true;

    }

    private void Start()
    {
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
        StartCoroutine(StalactitaAnimation());
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_triggerExitRoof)
        {
            return;
        }
        InstantiateParticles(_particleStalactitaPrefab, _particleSpawnStartPosition);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_triggerExitRoof)
        {
            return;
        }
        InstantiateParticles(_particleStalactitaPrefab, _particleSpawnStartPosition);
        gameObject.SetActive(false);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _triggerExitRoof = true;
        GetComponent<Collider2D>().isTrigger = false;
    }

    private void OnPlayerDetected()
    {
        StopAllCoroutines();
        //_animator.SetBool("trembling", false);
        _animator.enabled = false;

        _fallingSound.PlaySound();
        InstantiateParticles(_particleStalactitaStartPrefab, _particleSpawnPosition);
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _playerDetector.enabled = false;
        _playerKiller.SetCanKill(true);
        _fireDestroyer.SetCanDestroy(true);
        Debug.Log(name + " detected player");
    }

    private void ResetValues()
    {
        _animator.enabled = true;
        StartCoroutine(StalactitaAnimation());

        _rb.bodyType = RigidbodyType2D.Static;
        transform.position = _oPosition;
        _playerDetector.enabled = true;
        _fireDestroyer.SetCanDestroy(false);
        _playerKiller.SetCanKill(false);
        gameObject.SetActive(true);
        _triggerExitRoof = false;
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void RestartLevel()
    {
        Debug.Log("Stalact rs");
        ResetValues();
    }

    private void InstantiateParticles(GameObject _particlePrefab, Transform _particletransform)
    {
        GameObject particleObj = Instantiate(_particlePrefab, _particletransform.position, _particlePrefab.transform.rotation);

        ParticleSystem instantiateParticleSystem = particleObj.GetComponentInChildren<ParticleSystem>();

        instantiateParticleSystem.Play();
    }

    
    IEnumerator StalactitaAnimation()
    {
        while (true)
        {
            float randomDelay = Random.Range(2f, 5f);
            yield return new WaitForSeconds(randomDelay);

            _animator.SetBool("trembling", true);

            // Espera la duración de la animación

            //AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            //float _animationDuration = stateInfo.length;
            yield return new WaitForSeconds(1);

            // Desactiva el parámetro de animación aleatoria
            _animator.SetBool("trembling", false);

            yield return null;
        }
    }
}
