using System.Collections;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private GameObject pistolProjectile;
    [SerializeField] private Collider playerCollider;
    private Animator _animator;
    private bool _canFire = true;
    private const float Firedelay = 0.3f;
    private bool _isHolstered;

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
        if (Input.GetKeyDown(KeyCode.Mouse0) && _canFire && !_isHolstered)
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.A) && !_isHolstered)
        {
            Holster();
        }
        if (Input.GetKeyDown(KeyCode.Z) && _isHolstered)
        {
            Pull();
        }
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(Firedelay);
        _canFire = true;
    }

    private void Pull()
    {
        _animator.SetTrigger("Pull");
        _isHolstered = false;
    }

    private void Holster()
    {
        _animator.SetTrigger("Holster");
        _isHolstered = true;
    }

    private void Fire()
    {
        _canFire = false;
        _animator.SetTrigger("Fire");
        GameObject pistolProjectileInstance = Instantiate(pistolProjectile, gameObject.transform);
        pistolProjectileInstance.GetComponent<PistolProjectile>().SetPlayerCollider(playerCollider);
        StartCoroutine(ResetCooldown());
    }
}