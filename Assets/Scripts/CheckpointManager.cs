using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : Singleton<CheckpointManager>
{
    [SerializeField] public GameObject _spawnPoint;
    [SerializeField] private List<GameObject> _checkpoints;
    private void Start()
    {
        foreach (var checkpoint in _checkpoints)
        {
            checkpoint.GetComponent<Checkpoint>().OnCheckpointChange.AddListener(UpdateCheckpoint);
        }
    }

    private void UpdateCheckpoint(GameObject checkpoint)
    {
        _spawnPoint = checkpoint;
    }
}
