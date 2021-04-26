using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private LevelManager _levelManager;
    private bool _isPaused;

    private void Start()
    {
        EventManager.Listen("OnSceneLoaded", OnSceneLoaded);
        EventManager.Listen("OnSceneLoaded", OnSceneUnloaded);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (_isPaused)
            UnpauseGame();
        else
            PauseGame();
        
        EventManager.Invoke("TogglePause");
    }

    private void UnpauseGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
    }

    private void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0f;
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