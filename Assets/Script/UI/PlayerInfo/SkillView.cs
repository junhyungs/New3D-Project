using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModelViewViewModel;
using System.ComponentModel;
using UnityEngine.UI;
using EnumCollection;

namespace PlayerInfoUI
{
    [System.Serializable]
    public class SkillImage
    {
        [Header("SkillType")]
        public PlayerSkillType Type;
        [Header("SelectImage")]
        public GameObject SelectImage;
        [Header("IconImage")]
        public Image IconImage;
    }

    public class SkillView : View<SkillViewModel>, IOnAwakeRegisterView
    {
        [Header("Skill_Image"), SerializeField]
        private SkillImage[] _skillImages;
        private SkillImage _currentSkillImage;
        private Dictionary<PlayerSkillType, SkillImage> _skillImageDictionary
            = new Dictionary<PlayerSkillType, SkillImage>();

        [Header("Energe_Image"), SerializeField]
        private GameObject[] _activeEnerges;

        private const float MIN_ALPHA = 0.7f;
        private const float MAX_ALPHA = 1.0f;

        public void InitializeView()
        {
            InitializeDictionary();
            Initialize();
        }

        private void OnDisable()
        {
            ClearHandler();
        }

        protected override void OnPropertyChangedEvent(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(_viewModel.PlayerSkillType):
                    ChangeSkillImage(_viewModel.PlayerSkillType);
                    break;
                case nameof(_viewModel.SkillCost):
                    ChangeEnergeImage(_viewModel.SkillCost);
                    break;
            }
        }

        private void InitializeDictionary()
        {
            foreach(var image in _skillImages)
            {
                var key = image.Type;
                _skillImageDictionary.Add(key, image);
            }
        }

        private void ChangeSkillImage(PlayerSkillType playerSkillType)
        {
            if (_currentSkillImage != null)
                EnableSkillImage(_currentSkillImage, MIN_ALPHA);
            
            var nextSkillImage = GetSkillImage(playerSkillType);
            if(nextSkillImage != null)
            {
                EnableSkillImage(nextSkillImage, MAX_ALPHA);
                _currentSkillImage = nextSkillImage;
            }
        }

        private SkillImage GetSkillImage(PlayerSkillType playerSkillType)
        {
            if (!_skillImageDictionary.TryGetValue(playerSkillType, out var skillImage))
                return null;

            return skillImage;
        }

        private void EnableSkillImage(SkillImage skillImage, float targetAlpha)
        {
            bool activeSelf = skillImage.SelectImage.activeSelf ? false : true;
            skillImage.SelectImage.SetActive(activeSelf);

            AlphaChange(skillImage.IconImage, targetAlpha);
        }

        private void AlphaChange(Image image, float targetAlpha)
        {
            var color = image.color;

            color.a = targetAlpha;
            image.color = color;
        }

        private void ChangeEnergeImage(int cost)
        {
            ResetEnergeImage();

            for (int i = 0; i < cost; i++)
                _activeEnerges[i].SetActive(true);
        }

        private void ResetEnergeImage()
        {
            foreach (var image in _activeEnerges)
                image.SetActive(false);
        }
    }
}

