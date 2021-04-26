using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public bool _isGamePaused;
    public EventManager.OnGamePauseEvent OnGamePauseEvent;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        
        // Testing
        if (Input.GetKeyDown(KeyCode.K))
        {
            AudioManager._instance.PlayMusic(AudioManager._instance.musicExample);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            AudioManager._instance.PlaySoundEffect(AudioManager._instance.soundEffectExample);
        }
    }

    private void TogglePause()
    {
        if (_isGamePaused)
            UnpauseGame();
        else
            PauseGame();
        
        OnGamePauseEvent.Invoke(_isGamePaused);
    }

    private void UnpauseGame()
    {
        _isGamePaused = false;
        Time.timeScale = 1f;
    }

    private void PauseGame()
    {
        _isGamePaused = true;
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