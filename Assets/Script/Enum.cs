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

    public enum ObjectKey//PathCSV ID와 동일하게.
    {
        PlayerArrowPrefab,
        PlayerFireBallPrefab,
        PlayerBombPrefab,
        PlayerHookPrefab,
        PlayerSwordPrefab,
        PlayerHammerPrefab,
        PlayerDaggerPrefab,
        PlayerGreatSwordPrefab,
        PlayerUmbrellaPrefab

    }
    //Path 데이터로 사용되는 경우 DataKey와 ObjectKey는 일치해야함.
    public enum DataKey //DataCSV ID와 동일하게.
    {
        Player,
        Witch_Description,
        Swampking_Description,
        Betty_Description,
        HealthCrystal_Description,
        MagicCrystal_Description,
        Sword_Description,
        Hammer_Description,
        Dagger_Description,
        GreatSword_Description,
        Umbrella_Description,
        Ring_Description,
        RustyKey_Description,
        Teddy_Description,
        Trowel_Description,
        Surveillance_Description,
        Sword_Data,
        Hammer_Data,
        Dagger_Data,
        GreatSword_Data,
        Umbrella_Data,
        Default
    }

    public enum JsonData
    {
        New_3D_Player,
        New_3D_ScreenResolution,
        New_3D_Path,
        New_3D_PlayerSkill,
        New_3D_ItemDescription,
        New_3D_PlayerWeapon
    }

    public enum UIEvent
    {
        SaveInfoSlot_1,
        SaveInfoSlot_2,
        SaveInfoSlot_3,
        SkillView,
        SkillCostView,
        HealthView
    }

    public enum PlayerSkillType
    {
        PlayerBow = 1,
        PlayerFireBall,
        PlayerBomb,
        PlayerHook
    }

    public enum ItemType
    {
        Witch,
        Swampking,
        Betty,
        HealthCrystal,
        MagicCrystal,
        Sword,
        Hammer,
        Dagger,
        GreatSword,
        Umbrella,
        Ring,
        RustyKey,
        Teddy,
        Trowel,
        Surveillance
    }

    public enum PlayerHand
    {
        Idle,
        Right,
        Left,
        Charge_L,
        Charge_R
    }
}

