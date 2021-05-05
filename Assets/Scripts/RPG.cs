using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG : MonoBehaviour
{
    [SerializeField]
    private AudioClip _rpgSoundEffect;
    [SerializeField]
    private GameObject _rocketPrefab;
    
    private Animator _animator;
    
    private void Start()
    {
        LoadComponents();
    }

    private void LoadComponents()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _animator.SetTrigger("Fire");
            Instantiate(_rocketPrefab, transform.parent);
            AudioManager._instance.PlaySoundEffect(_rpgSoundEffect);
        }
    }
}
