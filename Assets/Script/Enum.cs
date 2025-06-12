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
        Default
    }

    public enum JsonData
    {
        New_3D_Player,
        New_3D_ScreenResolution,
        New_3D_Path,
        New_3D_PlayerSkill,
        New_3D_ItemDescription
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
        Right,
        Left,
        Idle
    }
}

