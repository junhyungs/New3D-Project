using EnumCollection;
using PlayerComponent;
using SO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : Singleton_MonoBehaviour<WeaponManager>
{
    [Header("WeaponTransforms"), SerializeField]
    private WeaponTransform[] _weaponTransforms;
    private EquipTransformInfo _equipTransformInfo;
    public EquipTransformInfo EquipTransformInfo
    {
        get => _equipTransformInfo;
        set
        {
            if(_equipTransformInfo == null)
            {
                _equipTransformInfo = value;
                InitializeWeaponInfo();
            }
        }
    }

    private Dictionary<ItemType, Dictionary<PlayerHand, WeaponTransform>> _weaponTransformDictionary
            = new Dictionary<ItemType, Dictionary<PlayerHand, WeaponTransform>>();
    private HashSet<ItemType> _weaponTypeSet;
    private GameObject[] _weaponArray;

    private IWeapon _currentWeapon;
    private ItemType _currentWeaponType;
    private WeaponInfo[] _weaponInfos;

    private struct WeaponInfo
    {
        public Transform Parent;
        public PlayerHand Hand;

        public WeaponInfo(Transform parent, PlayerHand hand)
        {
            this.Parent = parent;
            this.Hand = hand;
        }
    }

    private void Awake()
    {
        InitializeDictionary();
        InitializeHashSet();
    }

    private void InitializeDictionary()
    {
        foreach (var weaponTransform in _weaponTransforms)
        {
            var weaponType = weaponTransform.WeaponType;
            var handType = weaponTransform.HandType;

            if (!_weaponTransformDictionary.TryGetValue(weaponType, out var handDictionary))
            {
                handDictionary = new Dictionary<PlayerHand, WeaponTransform>();
                _weaponTransformDictionary.Add(weaponType, handDictionary);
            }

            handDictionary.TryAdd(handType, weaponTransform);
        }
    }

    private void InitializeHashSet()
    {
        _weaponTypeSet = new HashSet<ItemType>()
        {
            ItemType.Sword,
            ItemType.Hammer,
            ItemType.Dagger,
            ItemType.GreatSword,
            ItemType.Umbrella
        };
    }

    private void InitializeWeaponInfo()
    {
        _weaponInfos = new WeaponInfo[]
        {
            new(EquipTransformInfo.Holster, PlayerHand.Idle),
            new(EquipTransformInfo.WeaponR, PlayerHand.Right),
            new(EquipTransformInfo.WeaponL, PlayerHand.Left),
            new(EquipTransformInfo.WeaponR, PlayerHand.Charge_R),
            new(EquipTransformInfo.WeaponL, PlayerHand.Charge_L),
        };
    }

    private WeaponTransform GetWeaponTransform(ItemType type, PlayerHand hand)
    {
        if (_weaponTransformDictionary.TryGetValue(type, out var handDictionary))
        {
            if (handDictionary.TryGetValue(hand, out var weaponTransform))
                return weaponTransform;
        }

        return null;
    }

    public void SetWeapon(ItemType itemType)
    {
        if (!_weaponTypeSet.Contains(itemType))
            return;

        var player = PlayerManager.Instance.Player;
        
        if(_weaponArray != null)
            WeaponPool.Instance.SetWeaponItem(_weaponArray, itemType);

        Component component = player.GetComponent<IWeapon>() as Component;
        if(component != null)
            Destroy(component);

        switch (itemType)
        {
            case ItemType.Sword:
                _currentWeapon = player.AddComponent<Sword>();
                break;
            case ItemType.Hammer:
                _currentWeapon = player.AddComponent<Hammer>();
                break;
            case ItemType.Dagger:
                _currentWeapon = player.AddComponent<Dagger>();
                break;
            case ItemType.GreatSword:
                _currentWeapon = player.AddComponent<GreatSword>();
                break;
            case ItemType.Umbrella:
                _currentWeapon = player.AddComponent<Umbrella>();
                break;
        }

        _weaponArray = WeaponPool.Instance.GetWeaponItem(itemType);
        
        for (int i = 0; i < _weaponInfos.Length; i++)
            EquipWeapon(_weaponArray[i], _weaponInfos[i].Parent, itemType, _weaponInfos[i].Hand);

        _weaponArray[0].SetActive(true); //나중에 각 Weapon 컴포넌트안에서 활성화 하도록 변경.
        _currentWeapon.InitializeWeapon(_weaponArray);
        _currentWeaponType = itemType;
    }

    private void EquipWeapon(GameObject weapon, Transform parent, ItemType itemType, PlayerHand hand)
    {
        weapon.transform.SetParent(parent);

        var weaponTransform = GetWeaponTransform(itemType, hand);
        var localPos = weaponTransform.LocalPosition;
        var localEuler = weaponTransform.LocalEulerAngles;

        weapon.transform.localPosition = new Vector3(localPos.x, localPos.y, localPos.z);
        weapon.transform.localRotation = Quaternion.Euler(localEuler.x, localEuler.y, localEuler.z);
    }
}

public interface IWeapon
{
    void UseWeapon();
    void InitializeWeapon(GameObject[] weaponArray);
}
