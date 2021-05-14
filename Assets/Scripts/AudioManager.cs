using System.Collections;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource _audioSource;
    private IEnumerator _musicCoroutine;

    private void Start()
    {
        GetComponents();
        GameManager._instance.OnGamePausedEvent.AddListener(ToggleMusic);
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
        _audioSource.volume = 0.05f;
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
        _audioSource.PlayOneShot(audioClip, 1f);
    }
}