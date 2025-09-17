using EnumCollection;

namespace PlayerComponent
{
    public class Roll : PlayerRollState<RollStateBehaviour>, ICharacterState<Roll>, IInitializeEnable
    {
        public Roll(Player player) : base(player)
        {
            _behaviour.RollState = this;
        }

        public void Init()
        {
            GetBehaviour();
            _behaviour.RollState = this;
        }

        public void OnStateEnter()
        {
            TriggerAnimation(_roll);
        }

        public void OnStateFixedUpdate()
        {
            if (IsRoll)
            {
                Movement();
            }
            else
                _stateHandler.ChangeIdleORMoveState();
        }
    }
}

