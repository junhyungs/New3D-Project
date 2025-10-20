using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMaterialController : MonoBehaviour
{
    [Header("DissolveTime")]
    [SerializeField] private float _dissolveTime;
    private const float MaxValue = 5f;
    private const float MinValue = -0.5f;

    private Material _doorMaterial;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        StartCoroutine(OpenDoor());
    }

    private void Init()
    {
        var meshRenderer = GetComponent<MeshRenderer>();
        if(meshRenderer != null)
            _doorMaterial = meshRenderer.material;
    }

    public IEnumerator OpenDoor()
    {
        yield return DissolveDoor(MaxValue);
    }

    public IEnumerator CloseDoor()
    {
        yield return DissolveDoor(MinValue);
    }

    private IEnumerator DissolveDoor(float targetValue)
    {
        var elapsed = 0f;
        var startValue = _doorMaterial.GetFloat("_NoiseValue");
        while(elapsed < _dissolveTime)
        {
            elapsed += Time.deltaTime;
            var currentValue = Mathf.SmoothStep(startValue, targetValue, elapsed / _dissolveTime);
            _doorMaterial.SetFloat("_NoiseValue", currentValue);
            yield return null;
        }

        _doorMaterial.SetFloat("_NoiseValue", targetValue);
    }

    //Mathf.Lerp : 지정된 속도로 목표까지 진행. ex) 처음부터 끝까지 지정된 속도로 걷기.
    //Mathf.SmoothStep : 처음엔 느리게, 중간은 빠르게, 마지막은 느리게 부드러운 변화. s자 곡선
    //시간은 고정이지만, 속도가 변화한다. ex) 걷기 -> 달리기 -> 걷기 
    //Mathf.SmoothDamp : 목표에 가까워질수록 감속. 지정된 시간 안에 정확히 목표에 도달하는 것이 아님.
    //ex) 달리기 -> 걷기 -> 정지. (물리효과)
}
