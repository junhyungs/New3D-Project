using InventoryUI;
using System.Collections.Generic;
using UnityEngine;

namespace ModelViewPresenter
{
    public class Presenter
    {
        public Presenter()
        {
            _modelDictionary = new Dictionary<GameObject, PlayerItemSlot>();
        }

        protected Dictionary<GameObject, PlayerItemSlot> _modelDictionary;
        public virtual void RequestUpdate(GameObject gameObject) { }
        protected virtual void UpdateView(PlayerItemSlot slot) { }
    }

    public class Presenter<T> : Presenter where T : IView
    {
        public Presenter(T view)
        {
            _view = view;
        }
        
        protected T _view;

        public override void RequestUpdate(GameObject gameObject)
        {
            if (_modelDictionary.TryGetValue(gameObject, out PlayerItemSlot slot))
            {
                UpdateView(slot);
                return;
            }

            var model = gameObject.GetComponent<PlayerItemSlot>();
            if (model == null)
                return;

            UpdateView(model);
            _modelDictionary.Add(gameObject, model);
        }

        protected override void UpdateView(PlayerItemSlot slot)
        {
            var data = slot.DescriptionData;
            _view.UpdateDescription(data);
        }
    }
}

