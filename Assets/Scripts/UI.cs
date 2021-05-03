using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Text _timer;
    private Image _velocity;
    private Rigidbody _rb;
    private void Start()
    {
        _timer = GameObject.Find("Timer").GetComponent<Text>();
        _velocity = GameObject.Find("VelocityBar").GetComponent<Image>();
        _rb = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _timer.text = GameManager._instance.GetTimer();
        _velocity.fillAmount = _rb.velocity.magnitude / 100f;
        _velocity.color = Color.HSVToRGB(Mathf.Clamp(_rb.velocity.magnitude / 100f, 0.01f, 0.99f), 1f, 1f);
        Debug.Log(_rb.velocity.magnitude);
    }
}
