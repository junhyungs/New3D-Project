using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapComponent;

public class SpikeDoor : MonoBehaviour, IHitTrigger
{
    [Header("Speed")]
    [SerializeField] private float _speed;

    public void HitTrigger()
    {
        StartCoroutine(OpenDoor());
    }

    private IEnumerator OpenDoor()
    {
        var endPos = transform.localPosition + new Vector3(0f, -3.5f, 0f);
        while(Vector3.Distance(transform.localPosition, endPos) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos,
                _speed * Time.deltaTime);

            yield return null;
        }

        transform.localPosition = endPos;
    }
}
