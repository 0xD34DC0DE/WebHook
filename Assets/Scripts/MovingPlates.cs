using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlates : MonoBehaviour
{
    [SerializeField]
    private float range = 100f;
    [SerializeField]
    private float speed = 2f;
    private int _direction = 1;
    private float z;

    private void Start()
    {
        z = transform.position.z;
    }
    
    private void Update()
    {
        CheckDirection();
        transform.localPosition += new Vector3(0f, 0f, speed * _direction);
    }

    private void CheckDirection()
    {
        if (transform.position.z > z + range)
            _direction = -1;
        else if (transform.position.z < z - range)
            _direction = 1;
    }
}
