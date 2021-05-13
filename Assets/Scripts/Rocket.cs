using UnityEngine;
public class Rocket : MonoBehaviour
{
    private Rigidbody _playerRigidbody;
    [SerializeField] private AudioClip explosionSoundEffect;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip fireSoundEffect;
    private void Start()
    {
        Destroy(gameObject, 10f);
        transform.parent = null;
        _playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
        AudioManager._instance.PlaySoundEffect(fireSoundEffect);
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
        
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        AudioManager._instance.PlaySoundEffect(explosionSoundEffect);
        Destroy(gameObject);
    }
}