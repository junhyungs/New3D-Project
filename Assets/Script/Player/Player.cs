using UnityEngine;

namespace PlayerComponent
{
    public class Player : MonoBehaviour
    {
        public PlayerInputHandler InputHandler { get; private set; }        
        public PlayerStateTransitionHandler StateHandler { get; private set; }

        private void Awake()
        {
            InitializeOnAwakePlayer();
        }

        private void InitializeOnAwakePlayer()
        {
            var stateMachine = GetComponent<PlayerStateMachine>();

            InputHandler = new PlayerInputHandler(this);
            StateHandler = new PlayerStateTransitionHandler(stateMachine, InputHandler);
        }
    }
}

