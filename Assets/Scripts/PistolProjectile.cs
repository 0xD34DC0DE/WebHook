using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolProjectile : MonoBehaviour
{
    [SerializeField] private AudioClip _fireSoundEffect;
    private Transform _transform;
    private Collider _collider;
    private void Start()
    {
        Destroy(gameObject, 10f);
        transform.parent = null;
        LoadComponents();
        AudioManager._instance.PlaySoundEffect(_fireSoundEffect);
    }

    private void LoadComponents()
    {
        _transform = gameObject.transform;
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        _transform.position += _transform.forward * 120f * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(!collision.gameObject.tag.Equals("Level"))
            Physics.IgnoreCollision(collision.collider, _collider);
    }
}
