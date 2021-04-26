using System.Collections;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource _audioSource;
    private bool _isMusicPlaying;
    private bool _isGamePaused;

    // Testing
    public AudioClip musicExample;
    public AudioClip soundEffectExample;
    
    private void Start()
    {
        InitializeComponents();
        GameManager._instance.OnGamePauseEvent.AddListener(PauseMusic);
    }

    private void InitializeComponents()
    {
        _audioSource = GetComponent<AudioSource>();

        // Testing
        musicExample = Resources.Load<AudioClip>("Music/music_1");
        soundEffectExample = Resources.Load<AudioClip>("SoundEffects/sound_1");
    }

    public void PlayMusic(AudioClip audioClip)
    {
        if (_isGamePaused) return;
        
        float audioClipLength = audioClip.length;
        _audioSource.clip = audioClip;
        _audioSource.volume = 0.2f;
        _audioSource.Play();

        _isMusicPlaying = true;
        StartCoroutine(StopMusicOnEnd(audioClipLength));
    }

    public void PauseMusic(bool isGamePaused)
    {
        if (isGamePaused)
            _audioSource.Pause();
        else
            _audioSource.Play();

        _isGamePaused = isGamePaused;
    }

    private IEnumerator StopMusicOnEnd(float audioClipLength)
    {
        yield return new WaitForSeconds(audioClipLength);
        _isMusicPlaying = false;
    }

    public void StopMusic()
    {
        if (!_isMusicPlaying) return;
        _audioSource.Stop();
        _isMusicPlaying = false;
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        if (_isGamePaused) return;
        _audioSource.PlayOneShot(audioClip, 0.8f);
    }
}