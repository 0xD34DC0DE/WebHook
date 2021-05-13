using System;
using UnityEngine;
using UnityEngine.UI;

public class UI : Singleton<UI>
{
    [SerializeField] private Text _timer;
    [SerializeField] private Text _record;
    [SerializeField] private Text _speed;
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private Image healthBar;

    private void Start()
    {
        Player._instance.OnPlayerHitTaken.AddListener(UpdateUI);
        _record.text = "RECORD: " + ScoreManager._instance.HighScore;
    }
    
    private void UpdateUI(int lives)
    {
        healthBar.fillAmount = lives > 0 ? (float) Decimal.Divide(lives, Player._instance.MaxLives) : 0f;
    }
    

    private void Update()
    {
        _timer.text = GameManager._instance.GetTimer();
        _speed.text = "SPEED: " + Mathf.Round(_playerRigidbody.velocity.magnitude);
    }
}
