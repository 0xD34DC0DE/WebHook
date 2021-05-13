using System;
using UnityEngine;
using UnityEngine.UI;

public class UI : Singleton<UI>
{
    [SerializeField] private Text _timer;
    [SerializeField] private Text _record;
    [SerializeField] private Text _speed;
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private GameObject[] lives = new GameObject[3];
    [SerializeField] private Image _hitMark;
    private bool _isHitMarkShowing = false;

    private void Start()
    {
        Player._instance.OnPlayerHitTaken.AddListener(UpdateUI);
        _record.text = "RECORD: " + ScoreManager._instance.HighScore;
    }

    public void ShowHitMark()
    {
        _hitMark.color = new Color(1.0f, 1.0f, 1.0f,1.0f);
        _isHitMarkShowing = true;
    }

    private void UpdateUI(int lives)
    {
        for (int i = 0; i < 3; i++)
        {
            if(i < lives)
                this.lives[i].gameObject.SetActive(true);
            else
                this.lives[i].gameObject.SetActive(false);
        }
    }
    

    private void Update()
    {
        _timer.text = GameManager._instance.GetTimer();
        _speed.text = "SPEED: " + Mathf.Round(_playerRigidbody.velocity.magnitude);
        HideHitMark();
    }

    private void HideHitMark()
    {
        if (_isHitMarkShowing)
        {
            _hitMark.color = new Color(1.0f, 1.0f, 1.0f, (_hitMark.color.a - 0.01f));
            if (_hitMark.color.a <= 0f)
            {
                _isHitMarkShowing = false;
            }
        }
    }
}
