using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerBodyRotation : MonoBehaviour
{
    [Header("RotationSetting")]
    [SerializeField] private float _rotSpeed;
    private Coroutine _rotCoroutine;

    public MonoBehaviour Owner { get; set; }

    public float RotSpeed
    {
        set { _rotSpeed = value; }
    }

    public bool Rotation
    {
        set
        {
            if (value)
            {
                if (_rotCoroutine != null)
                    Owner.StopCoroutine(_rotCoroutine);

                _rotCoroutine = Owner.StartCoroutine(RotationCoroutine());
            }
            else
            {
                if(_rotCoroutine != null)
                {
                    Owner.StopCoroutine(_rotCoroutine);
                    _rotCoroutine = null;
                }
            }
        }
    }

    private IEnumerator RotationCoroutine()
    {
        while (true)
        {
            Quaternion rotation = Quaternion.Euler(0f, _rotSpeed * Time.deltaTime, 0f);
            transform.localRotation = transform.localRotation * rotation;
            yield return null;
        }
    }
}
