using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool _isGamePaused;
    public EventManager.OnGamePaused OnGamePausedEvent;
    private bool _isInGame = false;
    private float _timer;
    private bool _isTimerPaused;

    private void Start()
    {
        LoadMainMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _isInGame)
        {
            TogglePause();
        }

        Tick();
    }

    private void Tick()
    {
        if (!_isInGame && !_isTimerPaused) return;
        _timer += Time.deltaTime;
    }

    public String GetTimer()
    {
        return TimeSpan.FromSeconds(_timer).ToString("hh\\:mm\\:ss\\:ff");
    }

    public bool IsGamePaused()
    {
        return _isGamePaused;
    }

    public void TogglePause()
    {
        if (_isGamePaused)
            UnpauseGame();
        else
            PauseGame();

        OnGamePausedEvent.Invoke(_isGamePaused);
    }

    private void UnpauseGame()
    {
        _isGamePaused = false;
        Time.timeScale = 1f;
        _isTimerPaused = false;
        HideCursor();
    }
    
    private void PauseGame()
    {
        _isGamePaused = true;
        Time.timeScale = 0f;
        _isTimerPaused = true;
        ShowCursor();
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadMainMenu()
    {
        StopMusic();
        LevelManager._instance.LoadScene("MainMenu");
        UnpauseGame();
        ShowCursor();
        _isInGame = false;
        AudioManager._instance.PlayMusic(AudioManager._instance._mainMenuMusic);
    }

    public void LoadLevel1()
    {
        StopMusic();
        LevelManager._instance.LoadScene("Level1");
        HideCursor();
        _isInGame = true;
        AudioManager._instance.PlayMusic(AudioManager._instance.level1Music);
    }

    private void StopMusic()
    {
        AudioManager._instance.StopMusic();
    }
}