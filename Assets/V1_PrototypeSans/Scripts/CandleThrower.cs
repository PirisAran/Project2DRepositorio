using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleThrower : MonoBehaviour
{
    PlayerInput _playerInput;
    Transform _candle;

    [SerializeField]
    Collider2D PickRangeCandle;

    [SerializeField]
    float MaxThrowRange = 5;

    [SerializeField]
    float MinThrowRange = 2;

    [SerializeField]
    float TimeToMaxThrow = 1.5f;

    float _throwStartTime;

    bool _isChargingThrow = false;

    Rigidbody2D _candleRb;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, MaxThrowRange);

        if (_isChargingThrow)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GetCurrentRange());
        }

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, MinThrowRange);

        Gizmos.color = Color.white;
    }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _candle = GetCandle();
        _candleRb = _candle.GetComponent<Rigidbody2D>();
    }

    private Transform GetCandle()
    {
        Transform candle;

        for (int i = 0; i < transform.childCount; i++)
        {
            candle = transform.GetChild(i);

            if (candle.GetComponent<Candle>())
                return candle;
            
        }

        return null;
    }

    private void OnEnable()
    {
        _playerInput.OnThrowStarted += OnThrowStarted;
        _playerInput.OnThrowFinished += OnThrowFinished;
    }

    private void OnDisable()
    {
        _playerInput.OnThrowStarted -= OnThrowStarted;
        _playerInput.OnThrowFinished -= OnThrowFinished;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_candle.parent != null)
            return;

        if (other = PickRangeCandle)
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        _candle.SetParent(transform);
        _candle.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Debug.Log("picked up");
    }

    private void OnThrowStarted()
    {
        if (_candle.parent == null)
            return;

        _isChargingThrow = true;
        _throwStartTime = Time.time;
    }

    private void OnThrowFinished()
    {
        Vector2 dir = GetMouseDir();
        float currentSpeed = GetCurrentSpeed();
        Throw(dir, currentSpeed);

        _isChargingThrow = false;

    }

    private Vector2 GetMouseDir()
    {
        return (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
    }

    private float GetCurrentSpeed()
    {
        float currentRange = GetCurrentRange();

        float currentSpeed = (currentRange * (_candleRb.gravityScale * Physics2D.gravity.magnitude)) / (Mathf.Sin(20));

        return currentSpeed;
    }

    private float GetCurrentRange()
    {
        var timeFraction = Mathf.Clamp01((Time.time - _throwStartTime) / (TimeToMaxThrow));
        return Mathf.Lerp(MinThrowRange, MaxThrowRange, timeFraction);
    }

    private void Throw(Vector2 dir, float speed)
    {
        _candle.parent = null;

        _candleRb.bodyType = RigidbodyType2D.Dynamic;
        _candleRb.velocity = dir * speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D candleRB = _candle.GetComponent<Rigidbody2D>();
        if (candleRB.velocity.y == 0 && candleRB.bodyType != RigidbodyType2D.Static)
            candleRB.velocity = Vector2.zero;
    }


}
