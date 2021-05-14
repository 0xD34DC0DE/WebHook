using UnityEngine;

public class Player : Singleton<Player>
{
    private int _lives = 5;
    private Rigidbody _rigidbody;
    public EventManager.OnPlayerHitTaken OnPlayerHitTaken;
    [SerializeField] private AudioClip hurtSoundEffect;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject camera;
    private GameObject _activeWeapon;
    public readonly int MaxLives = 5;

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager._instance.TogglePause();
            pauseMenu.SetActive(GameManager._instance.IsGamePaused());
        }
    }

    public void InflictDamage()
    {
        _lives--;
        OnPlayerHitTaken.Invoke(_lives);
        CheckHealth();
        AudioManager._instance.PlaySoundEffect(hurtSoundEffect);
    }

    public void PlayerOutOfBounds()
    {
        Vector3 newPosition = CheckpointManager._instance._spawnPoint.transform.position + Vector3.up * 1.5f;
        transform.position = newPosition;
        camera.transform.position = newPosition;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        InflictDamage();
    }

    private void CheckHealth()
    {
        if (_lives <= 0)
        {
            deathMenu.SetActive(true);
            GameManager._instance.FinishGame();
        }
    }

    public bool IsDead()
    {
        return _lives == 0;
    }
}
