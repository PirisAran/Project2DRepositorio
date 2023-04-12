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
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform candle = transform.GetChild(i);
            if (candle.GetComponent<Candle>())
            {
                _candle = candle;
                return;
            }
        }
    }

    private void OnEnable()
    {
        _playerInput.OnCandleThrown += OnCandleThrown;
    }

    private void OnDisable()
    {
        _playerInput.OnCandleThrown -= OnCandleThrown;
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
