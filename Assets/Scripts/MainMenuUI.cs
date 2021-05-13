using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MainMenuUI : HoverAnimation
{
    private Button _playButton;
    private Button _exitButton;

    private void Start()
    {
        GetComponents();
        SetListeners();
    }

    private void GetComponents()
    {
        _playButton = GameObject.Find("Play").GetComponent<Button>();
        _exitButton = GameObject.Find("Exit").GetComponent<Button>();
    }

    private void SetListeners()
    {
        _playButton.onClick.AddListener(Play);
        _exitButton.onClick.AddListener(Quit);
    }

    private void Play()
    {
        GameManager._instance.LoadLevel1();
    }

    private void Quit()
    {
        Application.Quit();
    }
}