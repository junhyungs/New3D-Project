using InventoryUI;
using System.Collections.Generic;
using UnityEngine;

namespace ModelViewPresenter
{
    public class Presenter
    {
        public Presenter()
        {
            _modelDictionary = new Dictionary<GameObject, Slot>();
        }

        protected Dictionary<GameObject, Slot> _modelDictionary;
        public virtual void RequestUpdate(GameObject gameObject) { }
        protected virtual void UpdateView(Slot slot) { }
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
            if (_modelDictionary.TryGetValue(gameObject, out Slot slot))
            {
                UpdateView(slot);
                return;
            }

            var model = gameObject.GetComponent<Slot>();
            if (model == null)
                return;

            UpdateView(model);
            _modelDictionary.Add(gameObject, model);
        }

        protected override void UpdateView(Slot slot)
        {
            var data = slot.DescriptionData;
            _view.UpdateDescription(data);
        }
    }
}

