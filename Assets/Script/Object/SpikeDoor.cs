using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapComponent;

public class SpikeDoor : MonoBehaviour, IHitInteraction_Door
{
    [Header("Speed")]
    [SerializeField] private float _speed;
    private Vector3 _startPos;

    [Header("UniqueID")]
    [SerializeField] private UniqueObjectID _uniqueObjectID;
    public UniqueObjectID UniqueObjectID => _uniqueObjectID;
    public GameObject GameObject => gameObject;

    private void Start()
    {
        _startPos = transform.localPosition;
    }

    public void OnHit()
    {
        var targetPos = transform.localPosition + new Vector3(0f, -3.5f, 0f);
        StartCoroutine(MoveDoor(targetPos));
    }

    public void ResetObject()
    {
        transform.localPosition = _startPos;
    }

    public void CloseDoor()
    {
        StartCoroutine(MoveDoor(_startPos));
    }

    private IEnumerator MoveDoor(Vector3 targetPos)
    {
        while(Vector3.Distance(transform.localPosition, targetPos) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos,
                _speed * Time.deltaTime);   
            yield return null;
        }

        transform.localPosition = targetPos;
    }
}
