using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : HoverAnimation
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button scoresButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Text level1Record;
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject scores;

    private void Start()
    {
        SetListeners();
        level1Record.text = ScoreManager._instance.HighScore;
    }
    
    private void SetListeners()
    {
        playButton.onClick.AddListener(Play);
        exitButton.onClick.AddListener(Quit);
        backButton.onClick.AddListener(ShowMain);
        scoresButton.onClick.AddListener(ShowScores);
    }

    private void ShowScores()
    {
        scores.SetActive(true);
        main.SetActive(false);
    }

    private void ShowMain()
    {
        main.SetActive(true);
        scores.SetActive(false);
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