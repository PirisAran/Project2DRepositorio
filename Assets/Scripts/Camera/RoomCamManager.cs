using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamManager : MonoBehaviour
{
    static RoomCamManager _instance;

    [SerializeField] float _shakeDuration = 0.5f, _shakeDeltaTime = 1.5f;

    Camera _mainCamera;

    List<Cinemachine.CinemachineVirtualCamera> _cams = new List<Cinemachine.CinemachineVirtualCamera>();

    Cinemachine.CinemachineVirtualCamera _currentRoomCam;

    IEnumerator _currentCoroutine;

    CinemachineBasicMultiChannelPerlin _currentCBMCP;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

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

    public void SetCurrentRoomCam(Cinemachine.CinemachineVirtualCamera camera)
    {
        var mainCamBrain = _mainCamera.GetComponent<CinemachineBrain>();
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


    public void StartShakeCamera(float intensity, float duration)
    {
        if (_currentCoroutine != null) return;

        _currentCoroutine = ShakeCamera(intensity, duration);
        StartCoroutine(_currentCoroutine);   
    }

    public void ShakeOnce(float intensity, float duration)
    {
        StartCoroutine(DoOneShake(intensity, duration));
    }

    IEnumerator ShakeCamera(float intensity, float duration)
    {
        while (true)
        {
            yield return new WaitForSeconds(_shakeDeltaTime);
            StartCoroutine(DoOneShake(intensity, duration));
        }
    }

    IEnumerator DoOneShake(float intensity, float duration)
    {
        if (_currentCBMCP != null)
        {
            Debug.Log("one shake");
            _currentCBMCP.m_AmplitudeGain = intensity;
            float timer = duration;
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
}