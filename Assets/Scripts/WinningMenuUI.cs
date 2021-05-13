using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WinningMenuUI : HoverAnimation
{
    [SerializeField] private Text timer;
    [SerializeField] private Button exit;
    [SerializeField] private Button replay;
    private void Start()
    {
        timer.text = "YOU FINISHED THE LEVEL IN " + GameManager._instance.GetTimer();
        SetListeners();
    }

    private void SetListeners()
    {
        exit.onClick.AddListener(GameManager._instance.LoadMainMenu);
        replay.onClick.AddListener(GameManager._instance.LoadLevel1);
    }
}
