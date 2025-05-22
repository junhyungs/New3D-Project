using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using EnumCollection;

namespace PlayerComponent
{
    public class PlayerState
    {
        public PlayerState(Player player)
        {
            GetComponent(player);
            SetPlayerData();
        }

        protected Player _player;
        protected PlayerStateTransitionHandler _stateHandler;
        protected PlayerInputHandler _inputHandler;
        protected PlayerSaveData _data;

        private void GetComponent(Player player)
        {
            _player = player;
            _inputHandler = player.InputHandler;
            _stateHandler = player.StateHandler;
        }

        private void SetPlayerData()
        {
            DataManager.Instance.AddToPlayerData(null); //테스트를 위한 임시 코드.
            var key = EnumCollection.Key.Player.ToString();
            _data = DataManager.Instance.GetData(key) as PlayerSaveData;
        }

        protected virtual void InputCheck() { }
    }
}

