using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dummy : MonoBehaviour, ITakeDamage
{
    [Header("Health"), SerializeField] private int _health;
    [Header("Material"), SerializeField] private Material _origin;
    [Header("Target"), SerializeField] private Transform _target;

    private Material _copyMaterial;
    private NavMeshAgent _agent;
    private bool _isMove;

    private const float MAXFLOAT = 0.5f;
    private const float MINFLOAT = -0.5f;

    private void Awake()
    {
        _health = 4;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        CopyMaterial();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _agent.SetDestination(_target.position);
            _isMove = true;
        }

        if (_isMove)
        {
            var current = _agent.acceleration;
            _agent.acceleration = Mathf.MoveTowards(current, 8f, 5f * Time.deltaTime);

            
            var distance = Vector3.Distance(transform.position, _target.position);
            if(distance <= _agent.stoppingDistance)
            {
                _isMove = false;
                _agent.speed = 0f;
                _agent.SetDestination(transform.position);
            }

        }
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
        Debug.Log("IntensityExit");
    }
}
