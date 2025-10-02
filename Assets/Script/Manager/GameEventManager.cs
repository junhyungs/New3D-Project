using EnumCollection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEvent
{
    void Complete(GameEvent gameEvent);
    void Fail() { }
}

public class GameEventManager : Singleton_MonoBehaviour<GameEventManager>
{
    private IGameEvent _currentEvent;    

    public void StartEvent(IGameEvent gameEvent)
    {
        _currentEvent = gameEvent;
    }

    public void CompleteEvent(GameEvent gameEvent)
    {
        if(_currentEvent != null)
            _currentEvent.Complete(gameEvent);
    }

    public void FailEvent()
    {
        if(_currentEvent != null)
            _currentEvent.Fail();
    }
}
