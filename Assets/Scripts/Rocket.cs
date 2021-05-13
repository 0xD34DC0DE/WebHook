using UnityEngine;
public class Rocket : MonoBehaviour
{
    private Rigidbody _playerRigidbody;
    [SerializeField] private AudioClip explosionSoundEffect;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip fireSoundEffect;
    private const float MaxDistance = 8f;
    private void Start()
    {
        Destroy(gameObject, 10f);
        transform.parent = null;
        _playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
        AudioManager._instance.PlaySoundEffect(fireSoundEffect);
    }

    private void Update()
    {
        transform.position += -transform.forward * 50f * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Checkpoint"))
        {
            Physics.IgnoreCollision(other, GetComponent<Collider>());
            return;
        }
        
        Vector3 hitPosition = transform.position;
        float distance = Vector3.Distance(hitPosition, _playerRigidbody.position);
        float distanceMul = ((8.0f - distance)/ 8.0f);

        if (distance < MaxDistance)
            _playerRigidbody.AddForce((distanceMul) * 1500f * transform.forward);
        
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        AudioManager._instance.PlaySoundEffect(explosionSoundEffect);
        Destroy(gameObject);
    }
}