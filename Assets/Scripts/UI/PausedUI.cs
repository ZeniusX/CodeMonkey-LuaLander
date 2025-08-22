using System;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

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
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManage_OnGamePaused;
        GameManager.Instance.OnGameUnPaused += GameManager_OnGameUnPaused;

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
