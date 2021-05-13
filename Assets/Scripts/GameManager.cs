using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool _isGamePaused;
    public EventManager.OnGamePaused OnGamePausedEvent;
    private bool _isInGame = false;
    private float _timer;
    private bool _isGameOver = false;
    private bool _isTimerPaused;
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip level1Music;

    private void Start()
    {
        LoadMainMenu();
    }

    private void Update()
    {
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
        UnpauseGame();
        ShowCursor();
        _isInGame = false;
        LevelManager._instance.LoadScene("MainMenu");
        AudioManager._instance.PlayMusic(mainMenuMusic);
    }

    public void LoadLevel1()
    {
        StopMusic();
        ResetTimer();
        UnpauseGame();
        HideCursor();
        _isInGame = true;
        LevelManager._instance.LoadScene("Level1");
        ScoreManager._instance.LoadHighScore();
        AudioManager._instance.PlayMusic(level1Music);
    }

    public void ResetTimer()
    {
        _timer = 0f;
    }

    private void StopMusic()
    {
        AudioManager._instance.StopMusic();
    }

    public void FinishGame()
    {
        _isInGame = false;
        _isGameOver = true;
        TogglePause();
        
        if(!Player._instance.IsDead())
            ScoreManager._instance.SaveHighScore(GetTimer());
    }
    
    public bool IsInGame
    {
        get => _isInGame;
    }
}