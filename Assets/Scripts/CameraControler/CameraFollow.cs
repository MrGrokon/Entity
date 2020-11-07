using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _transform, _TargetTransform;
    public string TargetName = "Avatar";
    private Vector3 _cameraOffset;
    private GameObject _leaderCell;


    private void Awake()
    {
        _transform = this.transform;
        _TargetTransform = GameObject.Find(TargetName).transform;
        _cameraOffset = new Vector3(_transform.position.x, _transform.position.y, _transform.position.z);
    }

    private void LateUpdate()
    {
        _transform.position =_TargetTransform.position + _cameraOffset;
    }
}
