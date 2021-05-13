using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    private int _lives = 3;
    private Rigidbody _rigidbody;
    public EventManager.OnPlayerHitTaken OnPlayerHitTaken;
    [SerializeField] private AudioClip hurtSoundEffect;
    [SerializeField] private GameObject pauseMenu;
    private GameObject _activeWeapon;
    
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("AlienProjectile"))
        {
            Destroy(other.gameObject);
            Hit();
        }
    }

    private void Hit()
    {
        _lives--;
        OnPlayerHitTaken.Invoke(_lives);
        AudioManager._instance.PlaySoundEffect(hurtSoundEffect);
    }
}
