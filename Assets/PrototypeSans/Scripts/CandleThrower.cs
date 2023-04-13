using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleThrower : MonoBehaviour
{
    PlayerInput _playerInput;
    Transform _candle;

    [SerializeField]
    Collider2D PickRangeCandle;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _candle = GetCandle();
    }

    private Transform GetCandle()
    {
        Transform candle;

        for (int i = 0; i < transform.childCount; i++)
        {
            candle = transform.GetChild(i);

            if (candle.GetComponent<Candle>())
            {
                return candle;
            }
        }

        return null;
    }

    private void OnEnable()
    {
        _playerInput.OnCandleThrown += OnCandleThrown;
    }

    private void OnDisable()
    {
        _playerInput.OnCandleThrown -= OnCandleThrown;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_candle.parent != null)
            return;

        if (other = PickRangeCandle)
        {
            _candle.SetParent(transform);
            _candle.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Debug.Log("picked");
        }
    }

    private void OnCandleThrown()
    {
        if (_candle.parent == null)
            return;

        _candle.parent = null;

        Vector2 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;

        Debug.Log(dir);

        Rigidbody2D candleRB = _candle.GetComponent<Rigidbody2D>();

        candleRB.bodyType = RigidbodyType2D.Dynamic;

        candleRB.velocity = dir * 10;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D candleRB = _candle.GetComponent<Rigidbody2D>();
        if (candleRB.velocity.y == 0)
            candleRB.velocity = Vector2.zero;
    }


}
