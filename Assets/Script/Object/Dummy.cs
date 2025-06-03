using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, ITakeDamage
{
    [Header("Health"), SerializeField] private int _health;
    [Header("Material"), SerializeField] private Material _origin;

    private Material _copyMaterial;

    private const float MAXFLOAT = 0.5f;
    private const float MINFLOAT = -0.5f;

    private void Awake()
    {
        _health = 4;
    }

    private void Start()
    {
        CopyMaterial();
    }

    private void CopyMaterial()
    {
        var meshRenderer = GetComponent<MeshRenderer>();
        _copyMaterial = Instantiate(_origin);

        meshRenderer.material = _copyMaterial;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if(_health <= 0)
        {
            StartCoroutine(DissolveEffect(3f));
            return;
        }

        StartCoroutine(IntensityChange());
    }

    private IEnumerator DissolveEffect(float maxTime)
    {
        float elapsedTime = 0f;
        float startValue = _copyMaterial.GetFloat("_NoiseValue");

        while (elapsedTime < maxTime)
        {
            elapsedTime += Time.deltaTime;
            var colorValue = Mathf.Lerp(startValue, MINFLOAT, elapsedTime / maxTime);
            _copyMaterial.SetFloat("_NoiseValue", colorValue);

            yield return null;
        }

        _copyMaterial.SetFloat("_NoiseValue", MINFLOAT);
    }

    private IEnumerator IntensityChange(float baseValue = 2f, float power = 3f)
    {
        var currentColor = _copyMaterial.GetColor("_Color");
        var intensityUpColor = currentColor * Mathf.Pow(baseValue, power);

        _copyMaterial.SetColor("_Color", intensityUpColor);
        yield return new WaitForSeconds(0.1f);
        _copyMaterial.SetColor("_Color", currentColor);
    }
}
