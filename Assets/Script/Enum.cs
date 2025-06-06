using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnumCollection
{
    public enum E_PlayerState
    {
        Idle,
        Move,
        Roll,
        RollSlash,
        Falling,
        Climbing,
        Attack,
        ChargeAttack,
        Skill
    }

    public enum ObjectKey//PathCSV ID�� �����ϰ�.
    {
        PlayerArrowPrefab,
        PlayerFireBallPrefab,
        PlayerBombPrefab,
        PlayerHookPrefab,

    }
    //Path �����ͷ� ���Ǵ� ��� DataKey�� ObjectKey�� ��ġ�ؾ���.
    public enum DataKey //DataCSV ID�� �����ϰ�.
    {
        Player,
        PlayerArrowPrefab,
        PlayerFireBallPrefab,
        PlayerBombPrefab,
        PlayerHookPrefab
    }

    public enum JsonData
    {
        New_3D_Player,
        New_3D_ScreenResolution,
        New_3D_Path,
        New_3D_PlayerSkill,
    }

    public enum UIEvent
    {
        SaveInfoSlot_1,
        SaveInfoSlot_2,
        SaveInfoSlot_3,
    }

    public enum PlayerSkillType
    {
        PlayerBow = 1,
        PlayerFireBall,
        PlayerBomb,
        PlayerHook
    }

    public enum InventroyPanelType { Witch, Swampking, Betty, HealthCrystal, MagicCrystal }
    public enum WeaponPanelType { Sword, Hammer, Dagger, GreatSword, Umbrella }
    public enum TrinketPanelType
    {
        Ring, RustyKey, Teddy, Trowel, Surveillance
    }
}

