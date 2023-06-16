using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamManager : MonoBehaviour
{
    static RoomCamManager _instance;

    [SerializeField] float _shakeIntensity = 1, _shakeDuration = 0.5f, _shakeDeltaTime = 1.5f;

    List<RoomWithCameraBehaviour> _rooms = new List<RoomWithCameraBehaviour>();

    CinemachineVirtualCamera _currentRoomCam;

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

    public void AddToRoomList(RoomWithCameraBehaviour camera)
    {
        _rooms.Add(camera);
    }

    public void SetCurrentRoomCam(RoomWithCameraBehaviour room)
    {
        var mainCamBrain = Camera.main.GetComponent<CinemachineBrain>();
        mainCamBrain.enabled = true;

        if (_rooms != null)
        {
            foreach (RoomWithCameraBehaviour r in _rooms)
                r.Cam.gameObject.SetActive(false);
        }

        _currentRoomCam = room.Cam;
        _currentRoomCam.gameObject.SetActive(true);
        _currentCBMCP = _currentRoomCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    public void StartShakeCamera(float intensity)
    {
        if (_currentCoroutine != null) return;

        _currentCoroutine = ShakeCamera(intensity);
        StartCoroutine(_currentCoroutine);   
    }

    public void ShakeOnce(float intensity)
    {
        StartCoroutine(DoOneShake(intensity));
    }

    IEnumerator ShakeCamera(float intensity)
    {
        while (true)
        {
            yield return new WaitForSeconds(_shakeDeltaTime);
            StartCoroutine(DoOneShake(intensity));
        }
    }

    IEnumerator DoOneShake(float intensity)
    {
        if (_currentCBMCP != null)
        {
            _currentCBMCP.m_AmplitudeGain = intensity;
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
}