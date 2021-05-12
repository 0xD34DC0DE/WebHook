using UnityEngine;
public class Rocket : MonoBehaviour
{
    private Rigidbody _playerRigidbody;
    [SerializeField] private AudioClip _explosionSoundEffect;
    [SerializeField] private GameObject _explosionEffect;
    private void Start()
    {
        Destroy(gameObject, 10f);
        _playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
        transform.parent = null;
    }

    private void Update()
    {
        transform.position += -transform.forward * 70f * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Vector3 hitPosition = transform.position;
        float distance = Vector3.Distance(hitPosition, _playerRigidbody.position);
        // 0 - 8 
        float distanceMul = ((8.0f - distance)/ 8.0f);
        
        if(distance < 8)
            _playerRigidbody.AddForce((distanceMul) * 700f * transform.forward);
        
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        AudioManager._instance.PlaySoundEffect(_explosionSoundEffect);
        Destroy(gameObject);
    }
}