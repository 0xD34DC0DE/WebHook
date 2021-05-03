using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Text _timer;
    private void Start()
    {
        _timer = GameObject.Find("Timer").GetComponent<Text>();
    }

    private void Update()
    {
        _timer.text = GameManager._instance.GetTimer();
    }
}
