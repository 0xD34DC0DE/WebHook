using System;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    /*
        GameManager's events
    */
    [Serializable]
    public class OnGamePaused : UnityEvent<bool>
    {
        private UnityEvent<bool> _unityEvent;
    };

    /*
        LevelManager's events
    */
    [Serializable]
    public class OnSceneLoaded : UnityEvent<string>
    {
    };

    [Serializable]
    public class OnSceneUnloaded: UnityEvent<string>
    {
    };
}