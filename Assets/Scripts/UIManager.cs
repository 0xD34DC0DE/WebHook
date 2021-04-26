public class UIManager : Singleton<UIManager>
{
    void Start()
    {
        GameManager._instance.OnGamePauseEvent.AddListener(TogglePauseMenu);
        gameObject.SetActive(false);
    }

    private void TogglePauseMenu(bool isGamePaused)
    {
        gameObject.SetActive(isGamePaused);
    }
}