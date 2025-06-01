using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PlayerComponent
{
    [System.Serializable]
    public class SkillInfo
    {
        public PlayerSkillType Type;
        public int AnimationCode;
        public Transform FireTransform;
        public GameObject SkillItem;
    }

    public class PlayerSkillSystem : MonoBehaviour, IUnbindAction
    {
        [Header("PlayerAnimationEvent"), SerializeField] private PlayerAnimationEvent _animationEvent;
        [Header("Skill_Info"), SerializeField] private SkillInfo[] _skillInfos;

        private Dictionary<PlayerSkillType, ISkill> _skillDictionary = new Dictionary<PlayerSkillType, ISkill>();
        private Dictionary<PlayerSkillType, SkillInfo> _infoDictionary = new Dictionary<PlayerSkillType, SkillInfo>();
        private Action _unbindAction;

        private ISkill _currentSkill;

        private void Awake()
        {
            InitializeInfoDictionary();
        }

        private void Start()
        {
            InitializeSkillDictionary();
            ChangeSkill("1");

            var player = GetComponentInParent<Player>();
            player.InputHandler.ChangeSkillEvent += ChangeSkill;
            _unbindAction += () => player.InputHandler.ChangeSkillEvent += ChangeSkill;
        }

        private void InitializeInfoDictionary()
        {
            foreach(var info in _skillInfos)
                _infoDictionary.Add(info.Type, info);
        }

        private void InitializeSkillDictionary()
        {
            var enumArray = (PlayerSkillType[])Enum.GetValues(typeof(PlayerSkillType));
            
            foreach(var enumValue in enumArray)
            {
                PlayerSkill playerSkill = null;

                switch (enumValue)
                {
                    case PlayerSkillType.Bow:
                        playerSkill = new Bow(_animationEvent);
                        break;
                    case PlayerSkillType.FireBall:
                        playerSkill = new FireBall(_animationEvent);
                        break;
                    case PlayerSkillType.Bomb:
                        playerSkill = new Bomb(_animationEvent);
                        break;
                    case PlayerSkillType.Hook:
                        playerSkill = new Hook(_animationEvent);
                        break;
                }

                var info = _infoDictionary[enumValue];
                playerSkill.InitializeSkill(info);

                _skillDictionary.Add(enumValue, playerSkill);
            }
        }

        private void ChangeSkill(string keyName)
        {
            if (!int.TryParse(keyName, out int key)
                || !Enum.IsDefined(typeof(PlayerSkillType), key))
                return;

            var skillType = (PlayerSkillType)key;
            if(_skillDictionary.TryGetValue(skillType, out var skill))
                _currentSkill = skill;
        }

        public ISkill GetSkill()
        {
            return _currentSkill;
        }

        public void Unbind()
        {
            _unbindAction?.Invoke();
        }
    }

    public interface ISkill
    {
        public bool RequiresReload { get; set; }
        public bool EndSkill { get; set; }
        public void Execute();
        public void Reloading();
        public void Fire();
        public void RemoveProjectile();
        public void OnUpdateSkill();
    }
}

