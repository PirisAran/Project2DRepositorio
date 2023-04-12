using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleThrower : MonoBehaviour
{
    PlayerInput _playerInput;
    Transform _candle;

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
        if (other.transform == _candle)
        {
            _candle.SetParent(transform);
            Debug.Log("picked");
        }
    }

    private void OnCandleThrown()
    {
        _candle.parent = null;


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    
}
