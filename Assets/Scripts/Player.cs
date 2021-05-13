using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    private int _lives = 5;
    private Rigidbody _rigidbody;
    public EventManager.OnPlayerHitTaken OnPlayerHitTaken;
    [SerializeField] private AudioClip hurtSoundEffect;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathMenu;
    private GameObject _activeWeapon;
    public readonly int MaxLives = 5;
    
    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager._instance.TogglePause();
            pauseMenu.SetActive(GameManager._instance.IsGamePaused());
        }
    }

    public void InflictDamage()
    {
        _lives--;
        OnPlayerHitTaken.Invoke(_lives);
        CheckHealth();
        AudioManager._instance.PlaySoundEffect(hurtSoundEffect);
    }

    private void CheckHealth()
    {
        if (_lives <= 0)
        {
            deathMenu.SetActive(true);
            GameManager._instance.FinishGame();
        }
    }

    public bool IsDead()
    {
        return _lives == 0;
    }

    public void ResetLives()
    {
        _lives = MaxLives;
    }
}
