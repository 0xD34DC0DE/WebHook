using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Virus : MonoBehaviour
{
    [SerializeField] private float aggroRadius = 50f;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private AudioClip hitMarkSound;
    [SerializeField] private GameObject alienExplosion;
    private int _lives;
    private Transform _playerTransform;
    private Transform _transform;

    void Start()
    {
        LoadComponents();
        StartCoroutine(Fire());
        _lives = 2;
    }

    void LoadComponents()
    {
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        _transform = GetComponent<Transform>();
    }

    private IEnumerator Fire()
    {
        yield return new WaitForSeconds(fireRate);
        CreateProjectile();
        StartCoroutine(Fire());
    }

    private void CreateProjectile()
    {
        if (IsPlayerInRange())
        {
            var prefab = Instantiate(bullet, _transform.position + _transform.forward, Quaternion.identity);
        }
    }
    
    private void FixedUpdate()
    {
        if (IsPlayerInRange())
            LookAtPlayer();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("PlayerProjectile"))
        {
            Destroy(collider.gameObject);
            Hit();
        }
    }

    public void Hit()
    {
        _lives--;
        AudioManager._instance.PlaySoundEffect(hitMarkSound);
        CheckLives();
    }
    
    private void CheckLives()
    {
        if (_lives == 0)
        {
            Instantiate(alienExplosion, _transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void LookAtPlayer()
    {
        _transform.rotation = Quaternion.Lerp(_transform.rotation, 
            Quaternion.LookRotation(new Vector3(_playerTransform.position.x - _transform.position.x, _playerTransform.position.y - _transform.position.y, _playerTransform.position.z - _transform.position.z)),
            Time.deltaTime/0.5f); 
    }

    private bool IsPlayerInRange()
    {
        bool isInDistance = (Vector3.Distance(_playerTransform.position, gameObject.transform.position) < aggroRadius);
        if (!isInDistance)
            return false;
        
        RaycastHit raycastHit;
        if(Physics.Raycast(transform.position, Vector3.Normalize(_playerTransform.position - transform.position), out raycastHit))
        {
            if (raycastHit.collider.gameObject.tag.Equals("Player"))
                return true;
        }

        return false;
    }
}
