using UnityEngine;
using UnityEngine.UI;

public class DeathMenuUI : HoverAnimation
{
    [SerializeField] private Button _retry;
    [SerializeField] private Button _exit;

    private void Start()
    {
        AddListeners();
    }

    private void AddListeners()
    {
        _retry.onClick.AddListener(Retry);
        _exit.onClick.AddListener(Exit);
    }

    private void Exit()
    {
        GameManager._instance.LoadMainMenu();
    }

    private void Retry()
    {
        GameManager._instance.LoadLevel1();
    }
}
