using EnumCollection;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class Player : MonoBehaviour, ITakeDamage
    {
        [Header("Info")]
        [SerializeField] private InteractionInfo _interactionInfo;
        [SerializeField] private EquipTransformInfo _equipTransform;

        public PlayerInputHandler InputHandler { get; private set; }    
        public PlayerHealth PlayerHealth { get; private set; }
        public PlayerStateTransitionHandler StateHandler { get; private set; }
        public PlayerInteraction Interaction { get; private set; }
        public PlayerPlane Plane { get; private set; }
        public PlayerSkillSystem SkillSystem { get; private set; }

        private List<IUnbindAction> _onDestroyInvokeList = new List<IUnbindAction>();

        private void Awake()
        {
            InitializeOnAwakePlayer();
        }

        private void Start()
        {
            InitializeOnStartPlayer();
        }

        private void OnDestroy()
        {
            OnDestroyPlayer();
        }

        private void InitializeOnAwakePlayer()
        {
            var stateMachine = GetComponent<PlayerStateMachine>();
            Plane = GetComponent<PlayerPlane>();
            SkillSystem = GetComponentInChildren<PlayerSkillSystem>();
            
            InputHandler = new PlayerInputHandler(this);
            StateHandler = new PlayerStateTransitionHandler(stateMachine, InputHandler);
            Interaction = new PlayerInteraction(this, _interactionInfo);
            
            AddUnbindList();
            MakeWeaponInfo();
        }

        private void MakeWeaponInfo()
        {
            var weaponInfos = new WeaponInfo[]
            {
                new(_equipTransform.Holster, PlayerHand.Idle),
                new(_equipTransform.WeaponR, PlayerHand.Right),
                new(_equipTransform.WeaponL, PlayerHand.Left),
                new(_equipTransform.WeaponL, PlayerHand.Charge_L),
                new(_equipTransform.WeaponR, PlayerHand.Charge_R),
            };

            WeaponManager.Instance.WeaponInfos = weaponInfos;
        }

        private void InitializeOnStartPlayer()
        {
            PlayerHealth = new PlayerHealth(StateHandler);

            var key = EnableUI.PlayerUI.ToString();
            UIManager.Instance.EnableUI(key);
        }

        private void AddUnbindList()
        {
            _onDestroyInvokeList = new List<IUnbindAction>()
            {
                InputHandler, StateHandler, Interaction, SkillSystem
            };
        }

        private void OnDestroyPlayer()
        {
            foreach (var item in _onDestroyInvokeList)
                item.Unbind();
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_interactionInfo.InteractionTransform.position, _interactionInfo.Range);

            //if (_debugBoxPos == Vector3.zero)
            //    return;

            //Gizmos.color = Color.red;
            //Matrix4x4 rotationMatrix = Matrix4x4.TRS(_debugBoxPos, _debugBoxRot, Vector3.one);
            //Gizmos.matrix = rotationMatrix;
            //Gizmos.DrawWireCube(Vector3.zero, _debugBoxSize);
        }

        public void TakeDamage(int damage)
        {
            PlayerHealth.TakeDamage(damage);
        }
    }

    [System.Serializable]
    public class InteractionInfo
    {
        public float Range;
        public Transform InteractionTransform;
        public LayerMask Target;
    }

    [System.Serializable]
    public class EquipTransformInfo
    {
        [Header("EquipTransform")]
        public Transform Holster;
        public Transform WeaponL;
        public Transform WeaponR;
    }

    public struct WeaponInfo
    {
        public Transform Parent;
        public PlayerHand Hand;

        public WeaponInfo(Transform parent, PlayerHand hand)
        {
            this.Parent = parent;
            this.Hand = hand;
        }
    }
}

