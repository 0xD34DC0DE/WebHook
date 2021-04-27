using UnityEngine;
public class GameManager : Singleton<GameManager>
{
    public bool isGamePaused;
    public EventManager.OnGamePaused OnGamePausedEvent;

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
        if (isGamePaused)
            UnpauseGame();
        else
            PauseGame();

        OnGamePausedEvent.Invoke(isGamePaused);
    }

    private void UnpauseGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
    }

    private void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
    }
}