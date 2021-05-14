using UnityEngine;

public class VirusProjectile : MonoBehaviour
{
    private Transform _playerTransform;
    
    private Transform _transform;
    
    [SerializeField] private AudioClip _fireSoundEffect;

    void Start()
    {
        LoadComponents();
        AimAtPlayer();
        Destroy(gameObject, 10f);
        AudioManager._instance.PlaySoundEffect(_fireSoundEffect);
    }
    
    private void LoadComponents()
    {
        _transform = gameObject.transform;
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void AimAtPlayer()
    {
        _transform.rotation = Quaternion.LookRotation(new Vector3(
            _playerTransform.position.x - _transform.position.x,
            _playerTransform.position.y - _transform.position.y,
            _playerTransform.position.z - _transform.position.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") || other.gameObject.tag.Equals("Level"))
        {
            other.gameObject.GetComponent<Player>().InflictDamage();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        _transform.position += _transform.forward * 100f * Time.deltaTime; 
    }
}