using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

public class HealthPlant : MonoBehaviour, IInteractionItem
{
    [Header("HealthValue"), SerializeField]
    private int _healthValue;

    private Animator _animator;
    private Coroutine _growCoroutine;
    private ItemType _itemType = ItemType.Seed;
    private readonly int _growAnim = Animator.StringToHash("IsGrow");
    private bool _isGrow;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (_growCoroutine != null)
            return;

        if (!_isGrow)
        {
            var inventoryManager = InventoryManager.Instance;
            bool isUsed = inventoryManager.CanUseCurrencyItem(_itemType, 1);
            if (isUsed)
            {
                _growCoroutine = StartCoroutine(DoGrowAnimation());
            }
        }
        else
        {
            var player = PlayerManager.Instance.PlayerComponent;
            var playerHealth = player.PlayerHealth;
            if (playerHealth.Health >= playerHealth.MAXHEALTH)
                return;

            playerHealth.Health += _healthValue;
            _growCoroutine = StartCoroutine(ReleaseFruitAnimation());
        }
    }

    private IEnumerator DoGrowAnimation()
    {
        _animator.SetBool(_growAnim, true);
        yield return new WaitUntil(() =>
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("Armature_Idle");
        });

        _isGrow = true;
        _growCoroutine = null;
    }

    private IEnumerator ReleaseFruitAnimation()
    {
        _animator.SetBool(_growAnim, false);
        yield return new WaitUntil(() =>
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("Armature_Ungrown");
        });

        _isGrow = false;
        _growCoroutine = null;
    }
}
