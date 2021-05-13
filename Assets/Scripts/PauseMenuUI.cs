using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuUI : HoverAnimation
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        SetListeners();
    }

    private void SetListeners()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(GameManager._instance.LoadLevel1);
        quitButton.onClick.AddListener(QuitLevel);
    }

    private void ResumeGame()
    {
        GameManager._instance.TogglePause();
        gameObject.SetActive(false);
    }

    private void QuitLevel()
    {
        GameManager._instance.LoadMainMenu();
    }
}
