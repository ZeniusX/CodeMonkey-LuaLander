using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private const int MUSIC_VOLUME_MAX = 10;

    public static MusicManager Instance { get; private set; }

    private static float musicTime;
    private static int musicVolume = 3;

    public event EventHandler OnMusicVolumeChange;

    private AudioSource musicAudioSource;

    private void Awake()
    {
        Instance = this;

        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.time = musicTime;
    }

    private void Start()
    {
        musicAudioSource.volume = GetMusicVolumeNormalized();
    }

    private void Update()
    {
        musicTime = musicAudioSource.time;
    }

    public void ChangeMusicVolume()
    {
        musicVolume = (musicVolume + 1) % MUSIC_VOLUME_MAX;
        musicAudioSource.volume = GetMusicVolumeNormalized();
        OnMusicVolumeChange?.Invoke(this, EventArgs.Empty);
    }

    public float GetMusicVolumeNormalized() => ((float)musicVolume) / MUSIC_VOLUME_MAX;

    public int GetMusicVolume() => musicVolume;
}
