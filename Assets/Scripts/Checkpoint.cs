using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    public EventManager.OnCheckpointChange OnCheckpointChange;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            OnCheckpointChange.Invoke(spawnPoint);
        }
    }
}
