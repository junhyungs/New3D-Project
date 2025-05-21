using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Speed"), SerializeField] private float _speed;
    [Header("MovePosObject"), SerializeField] private GameObject _movePos;

    private List<Transform> _childTransforms = new List<Transform>();
    private Transform _nextTransform;

    private bool _isMove;

    void Start()
    {
        AddChildTransform();
    }

    private void AddChildTransform()
    {
        foreach(Transform childTransform in _movePos.transform)
        {
            _childTransforms.Add(childTransform);
        }
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (!_isMove)
        {
            _nextTransform = _childTransforms[Random.Range(0, _childTransforms.Count)];
            _isMove = true;
        }

        transform.position = Vector3.MoveTowards(transform.position, _nextTransform.position, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _nextTransform.position) < 0.1f)
        {
            transform.position = _nextTransform.position;
            _isMove = false;
        }
    }
}
