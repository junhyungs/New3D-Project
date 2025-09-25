using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class LowerBodyController : MonoBehaviour
{
    private Coroutine _rotCoroutine;
    public MonoBehaviour Owner { get; set; }

    private void StopCoroutine()
    {
        if (_rotCoroutine == null)
            return;

        Owner.StopCoroutine(_rotCoroutine);
        _rotCoroutine = null;
    }

    public void RotateForDuration(float rotSpeed, float seconds, Action callBack = null)
    {
        if (_rotCoroutine != null)
            StopCoroutine();

        _rotCoroutine = Owner.StartCoroutine(StartRotateForDuration(rotSpeed, seconds, callBack));
    }

    private IEnumerator StartRotateForDuration(float rotSpeed, float seconds, Action callBack = null)
    {
        var rotateDirection = Vector3.down;
        var t = 0f;
        while(t < seconds)
        {
            t += Time.deltaTime;
            var deltaRotation = rotateDirection * rotSpeed * Time.deltaTime;
            transform.localRotation *= Quaternion.Euler(deltaRotation);

            yield return null;
        }

        callBack?.Invoke();
    }

    public void InfinitiRotation(Vector3 rotDir, bool value, float rotSpeed = 75f)
    {
        if (value)
        {
            if (_rotCoroutine != null)
                StopCoroutine();

            _rotCoroutine = Owner.StartCoroutine(StartInfinitiRotation(rotSpeed, rotDir));
        }
        else
            StopCoroutine();
    }

    private IEnumerator StartInfinitiRotation(float rotSpeed, Vector3 rotDir)
    {
        while (true)
        {
            var delta = rotSpeed * Time.deltaTime;
            transform.localRotation *= Quaternion.Euler(rotDir * delta);
            yield return null;
        }
    }

    public void RotateByAngle(float rotSpeed, Action callBack = null, float angle = 360f)
    {
        if (_rotCoroutine != null)
            StopCoroutine();

        _rotCoroutine = Owner.StartCoroutine(StartRotateByAngle(rotSpeed, angle, callBack));
    }

    private IEnumerator StartRotateByAngle(float rotSpeed, float angle, Action callBack = null)
    {
        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0f, angle, 0f);

        var rotated = 0f;
        var rotateDirection = Vector3.down;

        while(rotated < angle)
        {
            var delta = rotSpeed * Time.deltaTime;
            transform.localRotation *= Quaternion.Euler(delta * rotateDirection);

            rotated += delta;
            yield return null;
        }

        transform.localRotation = targetRotation;
        callBack?.Invoke(); 

        _rotCoroutine = null;
    }

    //var sign = Mathf.Sign(angle); //부호를 반환하는 메서드. 매개변수가 0 > value = 1, 0 < value = -1, 0 == 0
}
