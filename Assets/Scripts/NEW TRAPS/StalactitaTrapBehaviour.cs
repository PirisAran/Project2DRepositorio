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
        _rb = GetComponent<Rigidbody2D>();
        Init();
    }

    private void Init()
    {
        _rb.bodyType = RigidbodyType2D.Static;
        _playerKiller.SetCanKill(false);
        _fireDestroyer.SetCanDestroy(false);
        _oPosition = transform.position;
    }

    private void Start()
    {
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
    }

    private void OnPlayerDetected()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _playerDetector.enabled = false;
        _playerKiller.SetCanKill(true);
        _fireDestroyer.SetCanDestroy(true);
        Debug.Log(name + " detected player");
    }

    private void ResetValues()
    {
        _rb.bodyType = RigidbodyType2D.Static;
        transform.position = _oPosition;
        _playerDetector.enabled = true;
        _fireDestroyer.SetCanDestroy(false);
        _playerKiller.SetCanKill(false);
        gameObject.SetActive(true);
    }

    public void RestartLevel()
    {
        Debug.Log("Stalact rs");
        ResetValues();
    }
}
