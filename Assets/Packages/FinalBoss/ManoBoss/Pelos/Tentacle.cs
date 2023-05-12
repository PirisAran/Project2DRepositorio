using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    [SerializeField]
    int Lenth;
    [SerializeField]
    LineRenderer Tentaculo;
    [SerializeField]
    Vector3[] SegmentPoses;
    Vector3[] _segmentV;

    [SerializeField]
    Transform TargetDir;
    [SerializeField]
    float targetDist;
    [SerializeField]
    float smoothSpeed;
    [SerializeField]
    float TrailSpeed;


    [SerializeField]
    float wiggleSpeed;
    [SerializeField]
    float WiggleMagnitude;
    [SerializeField]
    Transform wiggleDir;
    private void Start()
    {
        Tentaculo.positionCount = Lenth;
        SegmentPoses = new Vector3[Lenth];
        _segmentV = new Vector3[Lenth];

    }

    private void Update()
    {
        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * WiggleMagnitude);
        SegmentPoses[0] = TargetDir.position;
        for (int i = 1; i < SegmentPoses.Length; i++)
        {
            SegmentPoses[i] = Vector3.SmoothDamp(SegmentPoses[i],SegmentPoses[i-1]+TargetDir.right*targetDist,ref _segmentV[i],smoothSpeed+i/TrailSpeed);
        }
        Tentaculo.SetPositions(SegmentPoses); 
    }
}
