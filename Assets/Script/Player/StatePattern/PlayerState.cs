using GameData;

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
        protected PlayerPlane _plane;

        private void GetComponent(Player player)
        {
            _player = player;
            _inputHandler = player.InputHandler;
            _stateHandler = player.StateHandler;
            _plane = player.Plane;
        }

        private void SetPlayerData()
        {
            DataManager.Instance.AddToPlayerData(null); //�׽�Ʈ�� ���� �ӽ� �ڵ�.
            var key = EnumCollection.Key.Player.ToString();
            _data = DataManager.Instance.GetData(key) as PlayerSaveData;
        }

        protected virtual void InputCheck() { }
    }
}

