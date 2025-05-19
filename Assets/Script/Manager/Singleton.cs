using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new T();
            }

            return _instance;
        }
    }
}

public class Singleton_MonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if(_instance == null)
                {
                    var gameObject = new GameObject(nameof(T));
                    _instance = gameObject.AddComponent<T>();
                }
            }

            return _instance;
        }
    }
}
