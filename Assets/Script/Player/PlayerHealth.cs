using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using GameData;

namespace PlayerComponent
{
    public class PlayerHealth
    {
        public PlayerHealth(PlayerStateTransitionHandler handler)
        {
            _dataKey = DataKey.Player.ToString();
            _eventKey = UIEvent.HealthView.ToString();
            _handler = handler;

            Initialize();
        }

        private PlayerStateTransitionHandler _handler;
        private string _dataKey;
        private string _eventKey;
        private int _health;
        public int MAXHEALTH => 4;

        public int Health
        {
            get => _health;
            set
            {
                _health = Mathf.Clamp(value, 0, MAXHEALTH);
                UIManager.TriggerUIEvent(_eventKey, _health);
            }
        }

        private void Initialize()
        {
            var data = DataManager.Instance.GetData(_dataKey) as PlayerSaveData;
            if(data != null)
                Health = data.Health;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if(Health <= 0)
            {
                //죽으면 사망 상태로 이동.
            }

            //아직 죽지 않았으면 Hit 상태로 이동.
        }
    }
}

