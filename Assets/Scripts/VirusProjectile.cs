using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class VirusProjectile : MonoBehaviour
{
    private Transform _playerTransform;
    private Transform _transform;
    private Collider _collider;
    private AudioClip _fireSoundEffect;

    void Start()
    {
        LoadComponents();
        AimAtPlayer();
        Destroy(gameObject, 10f);
        AudioManager._instance.PlaySoundEffect(AudioManager._instance._fireSoundEffect);
    }
    
    private void LoadComponents()
    {
        _transform = gameObject.transform;
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        _collider = GetComponent<Collider>();
    }

    private void AimAtPlayer()
    {
        _transform.rotation = Quaternion.LookRotation(new Vector3(
            _playerTransform.position.x - _transform.position.x,
            _playerTransform.position.y - _transform.position.y,
            _playerTransform.position.z - _transform.position.z));
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        _transform.position += _transform.forward * 50f * Time.deltaTime; 
    }
}