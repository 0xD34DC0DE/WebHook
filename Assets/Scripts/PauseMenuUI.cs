using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
        GameManager._instance.OnGamePausedEvent.AddListener(TogglePauseMenu);
    }

    private void TogglePauseMenu(bool isPaused)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
