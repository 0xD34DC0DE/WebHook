using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected GameObject nextWeapon;
    [SerializeField] protected GameObject weapon;
    [SerializeField] private float fireDelay = 0.5f;
    [SerializeField] private GameObject projectile;
    private bool _isSwitching = false;
    protected Animator animator;
    private bool _canFire = true;
    
    void Awake()
    {
        animator = transform.GetComponent<Animator>();
    }
    
    protected void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f && !_isSwitching)
        {
            Holster();
            _isSwitching = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && _canFire)
        {
            Fire();
        }
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(fireDelay);
        _canFire = true;
    }
    
    private void Fire()
    {
        _canFire = false;
        animator.SetTrigger("Fire");
        Instantiate(projectile, weapon.transform);
        StartCoroutine(ResetCooldown());
    }

    private void Holster()
    {
        animator.SetTrigger("Holster");
    }

    private void Pull()
    {
        if(!_isSwitching)
            animator.SetTrigger("Pull");
    }

    public void OnFinishHolster()
    {
        nextWeapon.SetActive(true);
        _isSwitching = false;
        gameObject.SetActive(false);
    }

    public void OnFinishPull()
    {
        _isSwitching = false;
    }
}
