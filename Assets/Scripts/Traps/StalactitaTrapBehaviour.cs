using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TecnocampusProjectII;
public class StalactitaTrapBehaviour : MonoBehaviour, IRestartLevelElement
{
    [Header("Scripts Utilizados")]
    [SerializeField] KillPlayer _killPlayer;
    [SerializeField] DetectPlayer _detectPlayer;
    
    Vector2 _oPosition;
    Rigidbody2D _rb;

    private void OnEnable()
    {
        _detectPlayer.OnPlayerDetected += OnPlayerDetected;
    }

    private void OnDisable()
    {
        _detectPlayer.OnPlayerDetected -= OnPlayerDetected;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _oPosition = transform.position;
        _killPlayer.SetCanKill(false);
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
        _detectPlayer.enabled = false;
    }

    private void ResetValues()
    {
        _rb.bodyType = RigidbodyType2D.Static;
        transform.position = _oPosition;
        gameObject.SetActive(true);
    }

    public void RestartLevel()
    {
        ResetValues();
    }
}
