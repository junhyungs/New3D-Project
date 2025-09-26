using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEvent
{
    void StartEvent();
    void FailEvent();
    void UpdateEvent() { }
    void Complete();
    bool IsCompleted { get;}
}

public class GameEventManager : Singleton_MonoBehaviour<GameEventManager>
{
    private IGameEvent _currentEvent;

    public void StartEvent(IGameEvent gameEvent)
    {
        _currentEvent = gameEvent;
        _currentEvent.StartEvent();
    }

    public void FailEvent()
    {
        if(_currentEvent != null)
            _currentEvent.FailEvent();
    }

    private void Update()
    {
        if(_currentEvent != null &&
            !_currentEvent.IsCompleted)
            _currentEvent.UpdateEvent();
    }
}
