using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform playerPosition;
    
    private void Start()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        playerPosition = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate() {
        transform.position = playerPosition.transform.position;
    }
}
