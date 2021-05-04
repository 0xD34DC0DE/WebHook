﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Virus : MonoBehaviour
{
    private const float AggroRadius = 25f;
    private Transform _playerTransform;
    private Transform _transform;
    private Vector3 _latestPlayerPosition;
    [SerializeField]
    private GameObject _bullet;

    void Start()
    {
        LoadComponents();
        StartCoroutine(Fire());
    }

    void LoadComponents()
    {
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        _transform = GetComponent<Transform>();
    }

    private IEnumerator Fire()
    {
        yield return new WaitForSeconds(2);
        if(IsPlayerInRange())
            Instantiate(_bullet, _transform.position, Quaternion.identity);
        StartCoroutine(Fire());
    }

    private void FixedUpdate()
    {
        if (IsPlayerInRange())
            LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        _transform.rotation = Quaternion.Lerp(_transform.rotation, 
            Quaternion.LookRotation(new Vector3(_playerTransform.position.x - _transform.position.x, _playerTransform.position.y - _transform.position.y, _playerTransform.position.z - _transform.position.z)),
            Time.deltaTime/0.5f); 
    }

    private bool IsPlayerInRange()
    {
        return (Vector3.Distance(_playerTransform.position, gameObject.transform.position) < AggroRadius);
    }

    // Debugging
    private void OnDrawGizmos()
    {
        if (IsPlayerInRange())
            _latestPlayerPosition = _playerTransform.position;
        
        Gizmos.color = new Color(1.0f, 1.0f, 0f, 0.5f);
        Gizmos.DrawSphere(gameObject.transform.position, AggroRadius);
        Debug.DrawLine(_transform.position, _latestPlayerPosition, Color.red);
    }
}