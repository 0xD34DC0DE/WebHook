using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _resumeButton;
    private Button _restartButton;
    private Button _quitButton;
    private GameObject _latestSelection;
    private const int FontSizeSelected = 22;
    private const int DefaultFontSize = 20;
    
    private void Start()
    {
        GetComponents();
        SetListeners();
        GameManager._instance.OnGamePausedEvent.AddListener(TogglePauseMenu);
        gameObject.SetActive(false);
    }

    private void TogglePauseMenu(bool isPaused)
    {
        gameObject.SetActive(isPaused);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (_latestSelection == pointerEventData.pointerCurrentRaycast.gameObject) return;
        _latestSelection = pointerEventData.pointerCurrentRaycast.gameObject;
        _latestSelection.GetComponent<Text>().fontSize = FontSizeSelected;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        _latestSelection.GetComponent<Text>().fontSize = DefaultFontSize;
        _latestSelection = null;
    }

    private void GetComponents()
    {
        _resumeButton = GameObject.Find("Resume").GetComponent<Button>();
        _restartButton = GameObject.Find("Restart").GetComponent<Button>();
        _quitButton = GameObject.Find("Quit").GetComponent<Button>();
    }

    private void SetListeners()
    {
        _resumeButton.onClick.AddListener(ResumeGame);
        _quitButton.onClick.AddListener(QuitLevel);
    }

    private void ResumeGame()
    {
        GameManager._instance.TogglePause();
    }

    private void QuitLevel()
    {
        GameManager._instance.LoadMainMenu();
    }
}
