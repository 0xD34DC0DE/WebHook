using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Virus : MonoBehaviour
{
    [SerializeField] private float aggroRadius = 50f;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private GameObject _bullet;
    private Transform _playerTransform;
    private Transform _transform;

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
        yield return new WaitForSeconds(fireRate);
        if (IsPlayerInRange())
        {
            var prefab = Instantiate(_bullet, _transform.position + _transform.forward, Quaternion.identity);
        }

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
        return (Vector3.Distance(_playerTransform.position, gameObject.transform.position) < aggroRadius);
    }
}
