using EnumCollection;
using GameData;
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
    public IWeapon CurrentWeapon => _currentWeapon;
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

    private void Start()
    {
        //DataManager.Instance.ParsePlayerWeaponData(); // 테스트 코드
        SetWeapon(ItemType.Sword);
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
            new(EquipTransformInfo.WeaponL, PlayerHand.Charge_L),
            new(EquipTransformInfo.WeaponR, PlayerHand.Charge_R),
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
        WeaponObjectController weaponController = new WeaponObjectController();

        for (int i = 0; i < _weaponInfos.Length; i++)
            EquipWeapon(weaponController, _weaponArray[i], _weaponInfos[i].Parent, itemType, _weaponInfos[i].Hand);
        
        _currentWeapon.InitializeWeapon(weaponController);
    }

    private void EquipWeapon(WeaponObjectController weapon, GameObject weaponObject, 
        Transform parent, ItemType itemType, PlayerHand hand)
    {
        weaponObject.transform.SetParent(parent);

        var weaponTransform = GetWeaponTransform(itemType, hand);
        var localPos = weaponTransform.LocalPosition;
        var localEuler = weaponTransform.LocalEulerAngles;

        weaponObject.transform.localPosition = new Vector3(localPos.x, localPos.y, localPos.z);
        weaponObject.transform.localRotation = Quaternion.Euler(localEuler.x, localEuler.y, localEuler.z);
        weapon.AddWepons(hand, weaponObject);
    }
}

public class WeaponObjectController
{
    private Dictionary<PlayerHand, GameObject> _weapons
        = new Dictionary<PlayerHand, GameObject>();

    private GameObject _currentWeapon;

    public void AddWepons(PlayerHand hand, GameObject weapon)
    {
        if (!_weapons.ContainsKey(hand))
            _weapons.Add(hand, weapon);
    }

    public void SetWeaponActive(PlayerHand hand)
    {
        if(_weapons.TryGetValue(hand, out var weapon) && weapon != null)
        {
            weapon.SetActive(true);
            _currentWeapon = weapon;
        }
    }

    public void DeActiveCurrentWeapon()
    {
        if(_currentWeapon != null)
            _currentWeapon.SetActive(false);
    }
    
    public void AllDisableWeapon()
    {
        foreach(var weapon in _weapons.Values)
        {
            if (weapon != null)
                weapon.SetActive(false);
        }
    }
}

public interface IWeapon
{
    void UseWeapon();
    void InitializeWeapon(WeaponObjectController weapon);
    PlayerWeaponData GetWeaponData();
    WeaponObjectController GetWeaponController();
}
