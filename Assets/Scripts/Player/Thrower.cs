using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    [SerializeField] LayerMask WhatIsFireCollision;
    [SerializeField] public FireController _fire;

    //components
    LineRenderer _lr;
    Runner _runner;
    
    //Inputs thrower
    [SerializeField]
    KeyCode ThrowKey = KeyCode.Mouse0;
    [SerializeField]
    KeyCode PickUpKey = KeyCode.E;
    [SerializeField]
    KeyCode CancelThrowKey = KeyCode.Mouse1;

    [SerializeField]
    public Collider2D PickUpCollider;
    public bool HasFire => _hasFire;
    bool _hasFire = true;
    [SerializeField]
    float MinThrowSpeed = 2, MaxThrowSpeed = 20;
    [SerializeField]
    float TimeMaxThrow = 0.5f;
    //[SerializeField]
    //float DeltaSpeed = 3.0f;
    //[SerializeField]
    //float ParabolicShootAngle = 45.0f;
    [SerializeField]
    int ParabolicShootMaxPoints = 200;
    [SerializeField]
    float ParabolicShootTime = 4.0f;
    float _throwStartTime;
    public bool IsChargingThrow => _isChargingThrow;
    bool _isChargingThrow = false;
    
    void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        _runner = GetComponent<Runner>();
    }

    // Update is called once per frame
    void Update()
    {
        ThrowInput();
        PickUpInput();
        UpdateThrow();
    }

    private void UpdateThrow() // Se llama cada update
    {
        _lr.positionCount = 0;
        if (_isChargingThrow)
        {
            //DIBUJA LA LINIA DEL LANZAMIENTO CON EL COMPONENTE LINE RENDERER (_lr)
            List<Vector3> l_Positions = GetParabolicPositions(_fire.transform.position, (Vector2.Angle(Vector2.right, GetMouseDir())) * Mathf.Deg2Rad,
                GetCurrentThrowSpeed(), ParabolicShootMaxPoints, ParabolicShootTime);
            int currentParabolicShootPoints = GetUnblockedParabolicShootPointsNumber(l_Positions);
            _lr.positionCount = currentParabolicShootPoints;
            _lr.SetPositions(l_Positions.ToArray());
        }
    }

    private int GetUnblockedParabolicShootPointsNumber(List<Vector3> l_Positions)
    {
        int count = 0;
        for (int i = 0; i < l_Positions.Count; i++)
        {
            count++;
            Vector3 point = l_Positions[i];
            bool isBlocked = Physics2D.OverlapCircleAll(point, 0.01f, WhatIsFireCollision).Length > 0;
            if (isBlocked) break;
        }
        return count;
    }

    private void ThrowInput()
    {
        //Si no tiene fuego, hace return directamente
        if (!_hasFire)
            return;
        if (_isChargingThrow && Input.GetKeyDown(CancelThrowKey))
        {
            CancelThrow();
        }
        if (Input.GetKeyDown(ThrowKey))
            ThrowFireStart();
        if (Input.GetKeyUp(ThrowKey))
            ThrowFireFinish();
    }

    private void ThrowFireStart()
    {
        //empieza a cargar el disparo y se guarda el momento de inicio
        _isChargingThrow = true;
        _throwStartTime = Time.time;
    }
    private void ThrowFireFinish()
    {
        if (!_isChargingThrow)
            return;
        //Se calcula la direccion, velocidad (dependiendo del tiempo) y se lanza el fuego.
        Vector2 dir = GetMouseDir();
        float currentThrowSpeed = GetCurrentThrowSpeed();
        ThrowFire(dir, currentThrowSpeed);
    }

    private void ThrowFire(Vector2 dir, float speed)
    {
        //se llama el metodo de FireController para lanzar el fuego, dandole dir y speed.
        _fire.BeThrown(dir, speed);
        //ya no tiene fuego y no esta cargando
        _isChargingThrow = false;
    }
    private void CancelThrow()
    {
        _isChargingThrow = false;
    }

    public void SetHasFire(bool v)
    {
        _hasFire = v;
        _runner.ChangeSpeed();
    }

    private void PickUpInput()
    {
        if (_hasFire)
            return;
        if (Input.GetKeyDown(PickUpKey))
            TryPickUp();
    }

    //Try pick uf the fire
    private void TryPickUp()
    {
        if (_fire.OnPickUpRange)
            PickUpFire();
    }

    private void PickUpFire()
    {
        _fire.BePickedUp();
    }

    private Vector2 GetMouseDir()
    {
        return (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
    }
    private float GetCurrentThrowSpeed()
    {
        float timeFraction = Mathf.Clamp01((Time.time - _throwStartTime) / (TimeMaxThrow));
        float speed = Mathf.Lerp(MinThrowSpeed, MaxThrowSpeed, timeFraction);
        return speed;
    }
    List<Vector3> GetParabolicPositions(Vector2 initPos, float AngleInRadians, float Speed, int MaxPoints, float MaxTime)
    {
        List<Vector3> l_Positions = new List<Vector3>();
        float l_SpeedX = Mathf.Cos(AngleInRadians) * Speed;
        float l_SpeedY = Mathf.Sin(AngleInRadians) * Speed;
        float l_PositionY = initPos.y;
        for (int i = 0; i <= MaxPoints; ++i)
        {
            float l_Time = (i / (float)MaxPoints) * MaxTime;
            float l_DeltaTime = MaxTime / (float)MaxPoints;
            Vector3 l_Position = new Vector3(initPos.x + l_SpeedX * l_Time, l_PositionY);
            l_Positions.Add(l_Position);
            l_PositionY += l_SpeedY * l_DeltaTime;
            l_SpeedY += Physics.gravity.y * l_DeltaTime;
        }
        return l_Positions;
    }
}

