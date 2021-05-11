using System;
using System.Collections;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField] private AudioClip _fireSoundEffect;
    [SerializeField] private GameObject _projectile;
    private Animator _animator;
    private bool _isHolstered = false;
    private const float FireDelay = 0.5f;
    private bool _canFire = true;
    
    private void Start()
    {
        LoadComponents();
    }

    private void LoadComponents()
    {
        _animator = transform.parent.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _canFire)
        {
            Fire();
        }else if (Input.GetKeyDown(KeyCode.X))
        {
            Holster();
        }else if (Input.GetKeyDown(KeyCode.C))
        {
            Pull();
        }
    }
    
    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(FireDelay);
        _canFire = true;
    }
    
    private void Pull()
    {
        if (_isHolstered)
        {
            _animator.SetTrigger("Pull");
            _isHolstered = false;
        }
    }

    private void Holster()
    {
        if (!_isHolstered)
        {
            _animator.SetTrigger("Holster");
            _isHolstered = true;
        }
    }

    private void Fire()
    {
        _canFire = false;
        _animator.SetTrigger("Fire");
        Instantiate(_projectile, gameObject.transform);
        StartCoroutine(ResetCooldown());
        AudioManager._instance.PlaySoundEffect(_fireSoundEffect);
    }
}