using GameData;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StartSceneUI
{
    public class UIReference : MonoBehaviour
    {
        [Header("MainPanel"), SerializeField] private MainMenu _mainMenu;
        [Header("GamePanel"), SerializeField] private GameMenu _gameMenu;
        [Header("OptionPanel"), SerializeField] private OptionMenu _optionMenu;

        public MainMenu MainMenu => _mainMenu;
        public GameMenu GameMenu => _gameMenu;
        public OptionMenu OptionMenu => _optionMenu;

        private async void Start()
        {
            await InitializeGameMenuAsync();
        }

        private async Task InitializeGameMenuAsync()
        {
            var loadCount = _gameMenu.SaveInfoLength;
            var task = new Task<PlayerSaveData>[loadCount];

            for (int i = 0; i < loadCount; i++)
            {
                task[i] = SaveManager.Instance.LoadPlayerSaveDataAsync(i);
            }

            PlayerSaveData[] savePlayerData = await Task.WhenAll(task);
            _gameMenu.SetSaveInfo(savePlayerData);
        }
    }

    public class MenuUI : MonoBehaviour
    {
        [Header("UIAction"), SerializeField] private InputActionReference _uiAction;

        protected UIReference _uiReference;

        private void Awake()
        {
            _uiReference = transform.GetComponentInParent<UIReference>();
        }

        public virtual void OnEnableUI()
        {
            _uiAction.action.Enable();
            _uiAction.action.performed += CallBackContext;
        }

        public virtual void OnDisableUI()
        {
            _uiAction.action.performed -= CallBackContext;
            _uiAction.action.Disable();
        }

        public virtual void CallBackContext(InputAction.CallbackContext context) { }
    }
}