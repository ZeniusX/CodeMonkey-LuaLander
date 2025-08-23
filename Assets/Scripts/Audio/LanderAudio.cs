using System;
using UnityEngine;

[RequireComponent(typeof(Lander))]
public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAudioSource;

    private Lander lander;

    private void Awake()
    {
        lander = GetComponent<Lander>();
    }

    private void Start()
    {
        lander.OnBeforeForce += Lander_OnBeforeForce;
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;

        SoundManager.Instance.OnSoundVolumeChange += SoundManager_OnSoundVolumeChange;
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;

        thrusterAudioSource.volume = SoundManager.Instance.GetSoundVolumeNormalized();
        thrusterAudioSource.Pause();
    }

    private void SoundManager_OnSoundVolumeChange(object sender, EventArgs e)
    {
        thrusterAudioSource.volume = SoundManager.Instance.GetSoundVolumeNormalized();
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e)
    {
        if (thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Pause();
        }
    }

    private void Lander_OnBeforeForce(object sender, EventArgs e)
    {
        thrusterAudioSource.Pause();
    }

    private void Lander_OnUpForce(object sender, EventArgs e)
    {
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }

    private void Lander_OnLeftForce(object sender, EventArgs e)
    {
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }

    private void Lander_OnRightForce(object sender, EventArgs e)
    {
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }
}
