using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    private Canvas _canvas;
    private Button _playButton;
    private Button _quitButton;

    private void Start()
    {
        GetComponents();
        SetListeners();
    }

    private void GetComponents()
    {
        _canvas = GetComponent<Canvas>();
        _playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        _quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
    }

    private void SetListeners()
    {
        _playButton.onClick.AddListener(Play);
        _quitButton.onClick.AddListener(Quit);
    }

    private void Play()
    {
        LevelManager._instance.LoadScene("Scene1");
    }

    private void Quit()
    {
        Application.Quit();
    }
}