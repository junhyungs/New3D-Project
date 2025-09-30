using EnumCollection;
using GameData;
using MapComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventClass
{
    public class BossEvent<TComponent, TMapType> : IGameEvent
        where TComponent : ITakeDamage
        where TMapType : IMap
    {
        public BossEvent(TComponent bossComponent)
        {
            _bossComponent = bossComponent;
        }

        private TComponent _bossComponent;
        
        public void Complete(GameEvent gameEvent)
        {
            var mapData = DataManager.Instance.GetData<MapData>(DataKey.Map_Data);
            if(mapData.LevelDictionary.TryGetValue(typeof(TMapType).Name, out var progress))
            {
                if (!progress.MapEventDictionary.ContainsKey(gameEvent))
                    progress.MapEventDictionary.Add(gameEvent, true);
            } 
        }

        public void Fail()
        {
            _bossComponent.TakeDamage(9999);
        }
    }
}

