using System.Collections.Generic;
using System;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, Delegate> _uiEventDictionary = new Dictionary<string, Delegate>();

    public void RegisterUIEvent<T>(string key, Action<T> action)
    {
        if(!_uiEventDictionary.ContainsKey(key))
            _uiEventDictionary.Add(key, action);
    }

    public void UnRegisterUIEvent<T>(string key, Action<T> action)
    {
        if (_uiEventDictionary.ContainsKey(key))
        {
            var delegateChain = Delegate.Remove(_uiEventDictionary[key], action);

            if(delegateChain == null || delegateChain.GetInvocationList().Length == 0)
                _uiEventDictionary.Remove(key);
            else
                _uiEventDictionary[key] = delegateChain;
        }
    }

    public void TriggerUIEvent<T>(string key, T value)
    {
        if(_uiEventDictionary.TryGetValue(key, out Delegate action))
        {
            (action as Action<T>)?.Invoke(value);
        }
    }
}
