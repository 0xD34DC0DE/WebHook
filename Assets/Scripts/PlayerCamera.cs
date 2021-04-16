using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    
    [SerializeField]
    private Transform player;

    void Update() {
        transform.position = player.transform.position;
    }
}
