using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamManager : MonoBehaviour
{
    static RoomCamManager _instance;

    [SerializeField] float _shakeIntensity = 1, _shakeDuration = 0.5f, _shakeDeltaTime = 1.5f;

    List<Cinemachine.CinemachineVirtualCamera> _cams = new List<Cinemachine.CinemachineVirtualCamera>();

    Cinemachine.CinemachineVirtualCamera _currentRoomCam;

    IEnumerator _currentCoroutine;

    CinemachineBasicMultiChannelPerlin _currentCBMCP;

    public static RoomCamManager GetCameraManager()
    {
        if (_instance == null)
        {
            _instance = InitCameraManager();
        }
        return _instance;
    }

    private static RoomCamManager InitCameraManager()
    {
        GameObject cameraManager = new GameObject("CameraManager");
        cameraManager.transform.position = Vector3.zero;
        return cameraManager.AddComponent<RoomCamManager>();
    }

    public void AddToCamList(Cinemachine.CinemachineVirtualCamera camera)
    {
        _cams.Add(camera);
    }

    public void SetCurrentCam(Cinemachine.CinemachineVirtualCamera camera)
    {
        var mainCamBrain = Camera.main.GetComponent<CinemachineBrain>();
        mainCamBrain.enabled = true;

        if (_cams != null)
        {
            foreach (Cinemachine.CinemachineVirtualCamera cam in _cams)
                cam.gameObject.SetActive(false);
        }

        _currentRoomCam = camera;
        _currentRoomCam.gameObject.SetActive(true);
        _currentCBMCP = _currentRoomCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    public void StartShakeCamera()
    {
        if (_currentCoroutine != null) return;

        _currentCoroutine = ShakeCamera();
        StartCoroutine(_currentCoroutine);   
    }

    public void ShakeOnce()
    {
        StartCoroutine(DoOneShake());
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
        if (_currentCBMCP != null)
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