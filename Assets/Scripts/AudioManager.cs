using System.Collections;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource _audioSource;
    private bool _isMusicPlaying;

    // Testing
    public AudioClip musicExample;
    public AudioClip soundEffectExample;
    
    private void Start()
    {
        GetComponents();
        LoadSounds();
        GameManager._instance.OnGamePausedEvent.AddListener(PauseMusic);
    }

    // Testing TODO: DELETE
    private void LoadSounds()
    {
        musicExample = Resources.Load<AudioClip>("Music/music_1");
        soundEffectExample = Resources.Load<AudioClip>("SoundEffects/sound_1");
    }
    
    private void GetComponents()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip audioClip)
    {
        if (GameManager._instance.isGamePaused) return;
        
        float audioClipLength = audioClip.length;
        _audioSource.clip = audioClip;
        _audioSource.volume = 0.2f;
        _audioSource.Play();

        _isMusicPlaying = true;
        StartCoroutine(StopMusicOnEnd(audioClipLength));
    }

    public void PauseMusic(bool isGamePaused)
    {
        if (GameManager._instance.isGamePaused)
            _audioSource.Pause();
        else
            _audioSource.Play();
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
        if (GameManager._instance.isGamePaused) return;
        _audioSource.PlayOneShot(audioClip, 0.8f);
    }
}