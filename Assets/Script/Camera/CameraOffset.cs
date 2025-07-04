using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraOffset", menuName = "ScriptableObject/CameraOffset")]
public class CameraOffset : ScriptableObject
{
    [Header("TargetOffset"), SerializeField]
    private Vector3 _followOffset;
    [Header("BlendSpeed"), SerializeField]
    private float _blendSpeed;
    [Header("FieldOfView"), SerializeField]
    private float _fieldOfView;

    public Vector3 FollowOffset => _followOffset;
    public float BlendSpeed => _blendSpeed;
    public float FieldOfView => _fieldOfView;
}
