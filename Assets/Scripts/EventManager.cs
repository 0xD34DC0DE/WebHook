using System;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    /*
        GameManager's events
    */
    [Serializable]
    public class OnGamePaused : UnityEvent<bool>
    {
    };

    /*
        LevelManager's events
    */
    [Serializable]
    public class OnSceneLoaded : UnityEvent<string>
    {
    };

    [Serializable]
    public class OnSceneUnloaded : UnityEvent<string>
    {
    };

    /*
        Player
    */
    [Serializable]
    public class OnPlayerHitTaken : UnityEvent<int>
    {
    };

    /*
        Virus
    */
    [Serializable]
    public class OnVirusHitTaken : UnityEvent
    {
    };
    
    /*
        Checkpoints 
    */
    [Serializable]
    public class OnCheckpointChange : UnityEvent<GameObject>
    {
    };
}