using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolProjectile : MonoBehaviour
{
    [SerializeField] private AudioClip fireSoundEffect;

    private void Start()
    {
        Destroy(gameObject, 10f);
        transform.parent = null;
        AudioManager._instance.PlaySoundEffect(fireSoundEffect);
    }
    
    private void Update()
    {
        transform.position += transform.forward * 120f * Time.deltaTime;
    }
    
    public void SetPlayerCollider(Collider playerCollider)
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), playerCollider);
    }
}
