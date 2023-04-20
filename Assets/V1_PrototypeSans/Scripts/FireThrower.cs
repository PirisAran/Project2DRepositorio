using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireThrower : MonoBehaviour
{
    PlayerInput _playerInput;

    [SerializeField]
    Fire Fire;
    [SerializeField]
    Collider2D PickRangeCandle;
    [SerializeField]
    float MaxThrowSpeed = 20;
    [SerializeField]
    float MinThrowSpeed = 2;
    [SerializeField]
    float TimeToMaxThrow = 1.5f;
    [Range (2, 20)]
    [SerializeField]
    public float DeltaSpeed = 3.0f;
    [SerializeField]
    public float ParabolicShootAngle = 45.0f;
    [SerializeField]
    public int ParabolicShootMaxPoints = 200;
    [SerializeField]
    public float ParabolicShootTime = 4.0f;
    float _throwStartTime;
    bool _isChargingThrow = false;

    Rigidbody2D _fireRb;
    public Action OnFirePickedUp;

    List<Vector3> GetParabolicPositions(float AngleInRadians, float Speed, int MaxPoints, float MaxTime)
    {
        List<Vector3> l_Positions = new List<Vector3>();
        float l_SpeedX = Mathf.Cos(AngleInRadians) * Speed;
        float l_SpeedY = Mathf.Sin(AngleInRadians) * Speed;
        float l_PositionY = transform.position.y;
        for (int i = 0; i <= MaxPoints; ++i)
        {
            float l_Time = (i / (float)MaxPoints) * MaxTime;
            float l_DeltaTime = MaxTime / (float)MaxPoints;
            Vector3 l_Position = new Vector3(transform.position.x+l_SpeedX*l_Time, l_PositionY);
            l_Positions.Add(l_Position);
            l_PositionY += l_SpeedY * l_DeltaTime;
            l_SpeedY += Physics.gravity.y * l_DeltaTime;
        }
        return l_Positions;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        List<Vector3> l_Positions = GetParabolicPositions(ParabolicShootAngle * Mathf.Deg2Rad, 
            DeltaSpeed, ParabolicShootMaxPoints, ParabolicShootTime);
        for (int i = 1; i < l_Positions.Count; ++i)
            Gizmos.DrawLine(l_Positions[i - 1], l_Positions[i]);
    }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _fireRb = Fire.GetComponent<Rigidbody2D>();
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


    private void OnTriggerStay2D(Collider2D other)
    {
        if (Fire.IsAttached)
            return;

        if (other == PickRangeCandle && _playerInput.TryPickUp)
            PickUp();
    }

    private void PickUp()
    {
        Fire.GetComponent<Fire>().AttachToPlayer();
        OnFirePickedUp?.Invoke();
        Debug.Log("picked up");
    }

    private void OnThrowStarted()
    {
        if (!Fire.IsAttached)
            return;
        _isChargingThrow = true;
        _throwStartTime = Time.time;
    }

    private void OnThrowFinished()
    {
        if (!Fire.IsAttached)
            return;
        Vector2 dir = GetMouseDirFromPlayer();
        float currentSpeed = GetCurrentSpeed();
        Throw(dir, currentSpeed);
        _isChargingThrow = false;
    }

    private Vector2 GetMouseDirFromPlayer()
    {
        return (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
    }

    private float GetCurrentSpeed()
    {
        var timeFraction = Mathf.Clamp01((Time.time - _throwStartTime) / (TimeToMaxThrow));
        var currentSpeed = Mathf.Lerp(MinThrowSpeed, MaxThrowSpeed, timeFraction);
        return currentSpeed;
    }

    private void Throw(Vector2 dir, float speed)
    {
        Fire.GetComponent<Fire>().DetachFromPlayer();
        _fireRb.velocity = dir * speed;
    }

    void Update()
    {
        if (_fireRb.velocity.y == 0 && _fireRb.bodyType != RigidbodyType2D.Static)
            _fireRb.velocity = Vector2.zero;
        
        if (_isChargingThrow)
        {
            List<Vector3> l_Positions = GetParabolicPositions((Vector2.Angle(Vector2.right, GetMouseDirFromPlayer())) * Mathf.Deg2Rad, GetCurrentSpeed(), ParabolicShootMaxPoints, ParabolicShootTime);
            for (int i = 1; i < l_Positions.Count; ++i)
                Debug.DrawLine(l_Positions[i - 1], l_Positions[i], Color.red);
        }
    }
}
