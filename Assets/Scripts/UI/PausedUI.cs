using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button soundVolumeButton;
    [SerializeField] private Button musicVolumeButton;

    [Space]
    [SerializeField] private TextMeshProUGUI soundVolumeTextMesh;
    [SerializeField] private TextMeshProUGUI musicVolumeTextMesh;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.UnpauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
        soundVolumeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeSoundVolume();
            soundVolumeTextMesh.text = $"SOUND: {SoundManager.Instance.GetSoundVolume()}";
        });
        musicVolumeButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeMusicVolume();
            musicVolumeTextMesh.text = $"MUSIC: {MusicManager.Instance.GetMusicVolume()}";
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManage_OnGamePaused;
        GameManager.Instance.OnGameUnPaused += GameManager_OnGameUnPaused;

        soundVolumeTextMesh.text = $"SOUND: {SoundManager.Instance.GetSoundVolume()}";
        musicVolumeTextMesh.text = $"MUSIC: {MusicManager.Instance.GetMusicVolume()}";

        Hide();
    }

    private void GameManage_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void GameManager_OnGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }

    private void Hide() => gameObject.SetActive(false);
}
