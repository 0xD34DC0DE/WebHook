using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    private int lives = 3;
    private Rigidbody _rigidbody;
    public EventManager.OnPlayerHitTaken OnPlayerHitTaken;
    [SerializeField] private AudioClip _hurtSoundEffect;

    private void Start()
    {
        
    }
    
    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("AlienProjectile"))
        {
            lives--;
            OnPlayerHitTaken.Invoke(lives);
            Destroy(other.gameObject);
            AudioManager._instance.PlaySoundEffect(_hurtSoundEffect);
        }
    }
}
