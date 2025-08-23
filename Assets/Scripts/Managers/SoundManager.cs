using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const int SOUND_VOLUME_MAX = 10;

    public static SoundManager Instance { get; private set; }

    private static int soundVolume = 5;

    public event EventHandler OnSoundVolumeChange;

    [SerializeField] private AudioClip coinPickupAudioClip;
    [SerializeField] private AudioClip fuelPickupAudioClip;
    [SerializeField] private AudioClip crashAudioClip;
    [SerializeField] private AudioClip landingSuccessAudioClip;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnCoinPickup(object sender, Lander.OnCoinPickupEventArgs e)
    {
        PlayClip(coinPickupAudioClip);
    }

    private void Lander_OnFuelPickup(object sender, EventArgs e)
    {
        PlayClip(fuelPickupAudioClip);
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.Success:
                PlayClip(landingSuccessAudioClip);
                break;
            default:
                PlayClip(crashAudioClip);
                break;
        }
    }

    private void PlayClip(AudioClip audioClip)
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
    }

    public void ChangeSoundVolume()
    {
        soundVolume = (soundVolume + 1) % SOUND_VOLUME_MAX;
        OnSoundVolumeChange?.Invoke(this, EventArgs.Empty);
    }

    public float GetSoundVolumeNormalized() => ((float)soundVolume) / SOUND_VOLUME_MAX;

    public int GetSoundVolume() => soundVolume;
}
