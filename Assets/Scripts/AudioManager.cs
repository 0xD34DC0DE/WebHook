using System.Collections;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource _audioSource;

    private IEnumerator _musicCoroutine;

    // Testing
    public AudioClip musicExample;
    public AudioClip soundEffectExample;
    public AudioClip _mainMenuMusic;

    private void Start()
    {
        GetComponents();
        LoadSounds();
        GameManager._instance.OnGamePausedEvent.AddListener(ToggleMusic);
    }

    // Testing TODO: DELETE
    private void LoadSounds()
    {
        musicExample = Resources.Load<AudioClip>("Music/music_1");
        soundEffectExample = Resources.Load<AudioClip>("SoundEffects/sound_1");
        _mainMenuMusic = Resources.Load<AudioClip>("Music/music_2");
    }

    private void GetComponents()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip audioClip)
    {
        if (GameManager._instance.IsGamePaused()) return;

        float audioClipLength = audioClip.length;
        _musicCoroutine = StopMusicOnEnd(audioClipLength);
        _audioSource.clip = audioClip;
        _audioSource.volume = 0.2f;
        _audioSource.Play();
        StartCoroutine(_musicCoroutine);
    }

    public void ToggleMusic(bool isGamePaused)
    {
        if (isGamePaused) 
            _audioSource.Pause();
        else
            _audioSource.Play();
    }

    private IEnumerator StopMusicOnEnd(float audioClipLength)
    {
        yield return new WaitForSeconds(audioClipLength);
        _audioSource.Stop();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
        _audioSource.clip = null;
        if(_musicCoroutine != null)
            StopCoroutine(_musicCoroutine);
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        if (GameManager._instance.IsGamePaused()) return;
        _audioSource.PlayOneShot(audioClip, 0.8f);
    }
}