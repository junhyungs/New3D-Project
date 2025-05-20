using System.Collections.Generic;
using System;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, Delegate> _uiEventDictionary = new Dictionary<string, Delegate>();
    private Dictionary<string, GameObject> _enableUIDictionary = new Dictionary<string, GameObject>();
    public static event Action<bool> LoadingUIController;

    public void StartLoadingUI(bool enable)
    {
        LoadingUIController?.Invoke(enable);
    }

    public void RegisterUI(string uiName, GameObject gameObject)
    {
        _enableUIDictionary.TryAdd(uiName, gameObject);
    }

    public void UnRegisterUI(string uiName)
    {
        if(_enableUIDictionary.ContainsKey(uiName))
            _enableUIDictionary.Remove(uiName);
    }

    public void EnableUI(string uiName)
    {
        if (_enableUIDictionary.TryGetValue(uiName, out GameObject gameObject))
            gameObject.SetActive(true);
    }

    public void DisableUI(string uiName)
    {
        if (_enableUIDictionary.TryGetValue(uiName, out GameObject gameObject))
            gameObject.SetActive(false);
    }

    public void AllDisableUI()
    {
        foreach(var ui in _enableUIDictionary.Values)
        {
            ui.SetActive(false);
        }
    }

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
