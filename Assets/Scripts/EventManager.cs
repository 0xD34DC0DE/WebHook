using System;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    [Serializable]
    public class OnGamePauseEvent : UnityEvent<bool>
    {
    };
}