using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class UpperBodyController : MonoBehaviour
{
    [Header("RotationConstraint")]
    [SerializeField] private RotationConstraint _rotationConstraint;
    private Transform _playerTransform;

    public RotationConstraint RotationConstraint => _rotationConstraint;

    private void Start()
    {
        var playerManager = PlayerManager.Instance;
        if (playerManager != null)
            _playerTransform = playerManager.PlayerObject.transform;
    }

    void Update()
    {
        if (_playerTransform == null)
            return;

        Vector3 dir = _playerTransform.position - transform.position;
        if(dir != Vector3.zero)
        {
            dir.y = 0f;
            Quaternion look = Quaternion.LookRotation(dir);

            transform.localRotation = Quaternion.Inverse(transform.parent.rotation) * look;
        }
    }
}
