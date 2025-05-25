using UnityEngine;

namespace PlayerComponent
{
    public class Player : MonoBehaviour
    {
        [Header("Interaction")]
        [Header("InteractionInfo"), SerializeField] private InteractionInfo _interactionInfo;

        public PlayerInputHandler InputHandler { get; private set; }        
        public PlayerStateTransitionHandler StateHandler { get; private set; }
        public PlayerStateMachine StateMachine { get; private set; } //삭제 보류.
        public PlayerInteraction Interaction { get; private set; }

        private void Awake()
        {
            InitializeOnAwakePlayer();
        }

        private void OnDestroy()
        {
            InputHandler.UnBindAction();
            StateHandler.UnRegisterEvent();
            Interaction.UnBindAction();
        }

        private void InitializeOnAwakePlayer()
        {
            StateMachine = GetComponent<PlayerStateMachine>();
            
            InputHandler = new PlayerInputHandler(this);
            StateHandler = new PlayerStateTransitionHandler(StateMachine, InputHandler);
            Interaction = new PlayerInteraction(this, _interactionInfo);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_interactionInfo.InteractionTransform.position, _interactionInfo.Range);
        }
    }

    [System.Serializable]
    public class InteractionInfo
    {
        public float Range;
        public Transform InteractionTransform;
        public LayerMask Target;
    }
}

