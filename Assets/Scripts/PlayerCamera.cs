using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform playerHead;

    void Update() {
        transform.position = playerHead.transform.position;
    }
}
