using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private LevelManager _levelManager;

    private void Start()
    {
        EventManager.Listen("OnSceneLoaded", OnSceneLoaded);
        EventManager.Listen("OnSceneLoaded", OnSceneUnloaded);
    }

    private void OnSceneLoaded()
    {
        print("Scene loaded");
    }

    private void OnSceneUnloaded()
    {
        print("Scene unloaded");
    }
}