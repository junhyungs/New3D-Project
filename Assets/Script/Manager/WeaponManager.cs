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
    private Dictionary<ItemType, Dictionary<PlayerHand, WeaponTransform>> _weaponTransformDictionary
            = new Dictionary<ItemType, Dictionary<PlayerHand, WeaponTransform>>();
    private HashSet<ItemType> _weaponTypeSet;
    private GameObject[] _weaponArray;

    private IWeapon _currentWeapon;
    public IWeapon CurrentWeapon => _currentWeapon;
    public ItemType WeaponType { get; private set; }
    public WeaponInfo[] WeaponInfos { get; set; }

    private void Awake()
    {
        InitializeDictionary();
        InitializeHashSet();
    }

    //private void Start()
    //{
    //    DataManager.Instance.ParsePlayerWeaponData(); // 테스트 코드
    //}

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

    private WeaponTransform GetWeaponTransform(ItemType type, PlayerHand hand)
    {
        if (_weaponTransformDictionary.TryGetValue(type, out var handDictionary))
        {
            if (handDictionary.TryGetValue(hand, out var weaponTransform))
                return weaponTransform;
        }

        return null;
    }

    public void SetWeapon(ItemType itemType, WeaponData weaponData)
    {
        if (!_weaponTypeSet.Contains(itemType))
            return;

        var playerObject = PlayerManager.Instance.PlayerObject;

        if (_weaponArray != null && _currentWeapon != null)
            WeaponPool.Instance.SetWeaponItem(_weaponArray, _currentWeapon.AddressableKey);

        Component component = playerObject.GetComponent<IWeapon>() as Component;
        if(component != null)
            Destroy(component);

        switch (itemType)
        {
            case ItemType.Sword:
                _currentWeapon = playerObject.AddComponent<Sword>();
                break;
            case ItemType.Hammer:
                _currentWeapon = playerObject.AddComponent<Hammer>();
                break;
            case ItemType.Dagger:
                _currentWeapon = playerObject.AddComponent<Dagger>();
                break;
            case ItemType.GreatSword:
                _currentWeapon = playerObject.AddComponent<GreatSword>();
                break;
            case ItemType.Umbrella:
                _currentWeapon = playerObject.AddComponent<Umbrella>();
                break;
        }

        StartCoroutine(GetWeaponArray(_currentWeapon, itemType, weaponData));
        WeaponType = itemType;
        weaponData.WeaponType = itemType;
    }

    private IEnumerator GetWeaponArray(IWeapon currentWeapon, ItemType itemType, WeaponData weaponData)
    {
        var addressKey = currentWeapon.AddressableKey;
        
        var weaponArray = WeaponPool.Instance.GetWeaponItem(addressKey);
        if(weaponArray == null)
        {
            WeaponPool.Instance.CreatePool(addressKey);
            yield return new WaitUntil(() => 
            WeaponPool.Instance.GetWeaponItem(addressKey) != null);
            weaponArray = WeaponPool.Instance.GetWeaponItem(addressKey);
        }

        _weaponArray = weaponArray;

        WeaponObjectController controller = new WeaponObjectController();
        for(int i = 0; i < WeaponInfos.Length; i++)
        {
            EquipWeapon(controller, _weaponArray[i],
                WeaponInfos[i].Parent, itemType, WeaponInfos[i].Hand);
        }

        currentWeapon.InitializeWeapon(controller, weaponData);
    }
    
    private void EquipWeapon(WeaponObjectController weaponObjectController, GameObject weaponObject,
        Transform parent, ItemType itemType, PlayerHand hand)
    {
        weaponObject.transform.SetParent(parent);

        var weaponTransform = GetWeaponTransform(itemType, hand);
        var localPos = weaponTransform.LocalPosition;
        var localEuler = weaponTransform.LocalEulerAngles;

        weaponObject.transform.localPosition = new Vector3(localPos.x, localPos.y, localPos.z);
        weaponObject.transform.localRotation = Quaternion.Euler(localEuler.x, localEuler.y, localEuler.z);
        weaponObjectController.AddWepons(hand, weaponObject);
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
    string AddressableKey { get; }
    void InitializeWeapon(WeaponObjectController weapon, WeaponData weaponData);
    WeaponData GetWeaponData();
    WeaponObjectController GetWeaponController();
}
