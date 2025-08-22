using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioClip coinPickupAudioClip;
    [SerializeField] private AudioClip fuelPickupAudioClip;
    [SerializeField] private AudioClip crashAudioClip;
    [SerializeField] private AudioClip landingSuccessAudioClip;

    private void Start()
    {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnCoinPickup(object sender, Lander.OnCoinPickupEventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickupAudioClip, Camera.main.transform.position);
    }

    private void Lander_OnFuelPickup(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(fuelPickupAudioClip, Camera.main.transform.position);
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.Success:
                AudioSource.PlayClipAtPoint(landingSuccessAudioClip, Camera.main.transform.position);
                break;
            default:
                AudioSource.PlayClipAtPoint(crashAudioClip, Camera.main.transform.position);
                break;
        }
    }
}
