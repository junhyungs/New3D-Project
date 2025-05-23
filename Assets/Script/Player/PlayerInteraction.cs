using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PlayerComponent
{
    public class PlayerInteraction
    {
        public PlayerInteraction(Player player, InteractionInfo info)
        {
            _info = info;

            InitializeDictionary();
            InitializeInteractionEvent(player);
        }

        private InteractionInfo _info;
        private Action _unbindAction;
        private Dictionary<Type, Action<IInteraction>> _interactDictionary;        
        private void InitializeDictionary()
        {
            _interactDictionary = new Dictionary<Type, Action<IInteraction>>()
            {
                { typeof(IInteractionGameObject), interaction => ((IInteractionGameObject)interaction).Interact()},
                { typeof(IInteractionItem), interaction => ((IInteractionItem)interaction).Interact() },
                { typeof(IInteractionDialog), interaction => ((IInteractionDialog)interaction).Interact() }
            };
        }

        private void InitializeInteractionEvent(Player player)
        {
            var inputHandler = player.InputHandler;
            inputHandler.InteractionEvent += Interaction;
            _unbindAction += () => inputHandler.InteractionEvent -= Interaction;
        }

        public void UnBindAction()
        {
            _unbindAction.Invoke();
        }

        public void Interaction()
        {
            Collider[] results = new Collider[1];

            var position = _info.InteractionTransform.position;
            var range = _info.Range;
            var layer = _info.Target;

            var targetCount = Physics.OverlapSphereNonAlloc(position, range, results, layer);
            if (targetCount == 0)
                return;

            IInteraction interaction = results[0].GetComponent<IInteraction>();
            if (interaction != null)
                InvokeInteraction(interaction);
        }

        private void InvokeInteraction(IInteraction interaction)
        {
            Type key = interaction switch
            {
                IInteractionGameObject => typeof(IInteractionGameObject),
                IInteractionDialog => typeof(IInteractionDialog),
                IInteractionItem => typeof(IInteractionItem),
                _ => throw new ArgumentException()
            };

            if (_interactDictionary.TryGetValue(key,
                out Action<IInteraction> action))
            {
                action?.Invoke(interaction);
            }
        }   
    }
}

