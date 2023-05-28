using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static CameraManager _cameraManager;

    [SerializeField] float _shakeIntensity = 1, _shakeDuration = 0.5f, _shakeDeltaTime = 1.5f;

    List<CinemachineVirtualCamera> _cameras = new List<CinemachineVirtualCamera>();

    CinemachineVirtualCamera _currentCamera;

    IEnumerator _currentCoroutine;

    CinemachineBasicMultiChannelPerlin _currentCBMCP;

    public static CameraManager GetCameraManager()
    {
        if (_cameraManager == null)
        {
            _cameraManager = InitCameraManager();
        }
        return _cameraManager;
    }

    private static CameraManager InitCameraManager()
    {
        GameObject cameraManager = new GameObject("CameraManager");
        cameraManager.transform.position = Vector3.zero;
        return cameraManager.AddComponent<CameraManager>();
    }

    public void AddToCameraList(CinemachineVirtualCamera camera)
    {
        _cameras.Add(camera);
    }

    public void SetCurrentCamera(CinemachineVirtualCamera camera)
    {
        foreach (CinemachineVirtualCamera cam in _cameras)
            cam.gameObject.SetActive(false);

        _currentCamera = camera;
        _currentCamera.gameObject.SetActive(true);
        _currentCBMCP = _currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    public void StartShakeCamera()
    {
        if (_currentCoroutine != null) return;

        _currentCoroutine = ShakeCamera();
        StartCoroutine(_currentCoroutine);   
    }

    IEnumerator ShakeCamera()
    {
        while (true)
        {
            yield return new WaitForSeconds(_shakeDeltaTime);
            StartCoroutine(DoOneShake());
        }
    }

    IEnumerator DoOneShake()
    {
        _currentCBMCP.m_AmplitudeGain = _shakeIntensity;
        float timer = _shakeDuration;
        while (timer > 0)
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0)
            {
                _currentCBMCP.m_AmplitudeGain = 0;
            }
            yield return null;
        }
    }

    public void StopShakeCamera()
    {
        if (_currentCoroutine == null) return;
        
        StopCoroutine(_currentCoroutine);
        _currentCoroutine = null;
    }
    private void Update()
    {
    }
}