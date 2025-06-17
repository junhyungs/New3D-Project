using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModelViewViewModel;
using EnumCollection;

namespace PlayerInfoUI
{
    public class SkillViewModel : ViewModel
    {
        public SkillViewModel()
        {
            _typeKey = UIEvent.SkillView;
            _costKey = UIEvent.SkillCostView;
        }

        private UIEvent _typeKey;
        private UIEvent _costKey;

        private PlayerSkillType _skillType;
        public PlayerSkillType PlayerSkillType
        {
            get => _skillType;
            set
            {
                if (_skillType == value)
                    return;

                _skillType = value;
                OnPropertyChanged(nameof(PlayerSkillType));
            }
        }

        private int _skillCost;
        public int SkillCost
        {
            get => _skillCost;
            set
            {
                if (_skillCost == value)
                    return;

                _skillCost = value;
                OnPropertyChanged(nameof(SkillCost));
            }
        }

        public void ChangeSkillType(PlayerSkillType skillType)
        {
            PlayerSkillType = skillType;
        }

        public void ChangeSkillCost(int cost)
        {
            SkillCost = cost;
        }

        public override void RegisterEvent()
        {
            var typeKey = _typeKey.ToString();
            var costKey = _costKey.ToString();

            UIManager.RegisterUIEvent<PlayerSkillType>(typeKey, ChangeSkillType);
            UIManager.RegisterUIEvent<int>(costKey, ChangeSkillCost);
        }

        public override void UnRegisterEvent()
        {
            var typeKey = _typeKey.ToString();
            var costKey = _costKey.ToString();

            UIManager.UnRegisterUIEvent<PlayerSkillType>(typeKey, ChangeSkillType);
            UIManager.UnRegisterUIEvent<int>(costKey, ChangeSkillCost);
        }
    }
}

