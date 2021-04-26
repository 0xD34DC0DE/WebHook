public class UIManager : Singleton<UIManager>
{
    void Start()
    {
        EventManager.Listen("TogglePause", togglePauseMenu);
        gameObject.SetActive(false);
    }

    private void togglePauseMenu()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}