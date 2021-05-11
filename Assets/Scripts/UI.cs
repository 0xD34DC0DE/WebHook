using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Text _timer;
    [SerializeField] private Text _speed;
    [SerializeField] private Rigidbody _playerRb;
    private void Start()
    {
        
    }

    private void Update()
    {
        _timer.text = GameManager._instance.GetTimer();
        _speed.text = "SPEED: " + Mathf.Round(_playerRb.velocity.magnitude);
    }
}
