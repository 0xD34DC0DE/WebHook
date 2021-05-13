using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private Canvas winningCanvas;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            winningCanvas.gameObject.SetActive(true);
            GameManager._instance.FinishGame();
        }
    }
}
