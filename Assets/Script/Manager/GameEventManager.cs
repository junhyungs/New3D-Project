using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEvent
{
    void StartEvent();
    void UpdateEvent() { }
    void Complete(bool success);
    bool IsCompleted { get;}
    bool IsSuccess { get; }
}

public class GameEventManager : Singleton_MonoBehaviour<GameEventManager>
{
    private IGameEvent _currentEvent;
    public void StartEvent(IGameEvent gameEvent)
    {
        _currentEvent = gameEvent;
        if(_currentEvent != null)
            _currentEvent.StartEvent();
    }

    private void Update()
    {
        if(_currentEvent != null &&
            !_currentEvent.IsCompleted)
            _currentEvent.UpdateEvent();
    }
}
