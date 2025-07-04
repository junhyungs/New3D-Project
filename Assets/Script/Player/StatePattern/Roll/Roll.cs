using EnumCollection;

namespace PlayerComponent
{
    public class Roll : PlayerRollState<RollStateBehaviour>, ICharacterState<Roll>, IEnableObject
    {
        public Roll(Player player) : base(player)
        {
            _behaviour.RollState = this;
        }

        public void OnEnableObject()
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

