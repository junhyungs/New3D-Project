using GameData;
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
        protected PlayerConstantData _constantData;
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
            DataManager.Instance.AddToPlayerData(null); //테스트를 위한 임시 코드.
            _data = DataManager.Instance.GetData(DataKey.Player.ToString()) as PlayerSaveData;
            _constantData = _data.ConstantData;
        }
    }
}

