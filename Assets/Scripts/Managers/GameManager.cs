using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private static int levelNumber = 1;
    private static int totalScore;

    public static void ResetStaticData()
    {
        levelNumber = 1;
        totalScore = 0;
    }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;

    [SerializeField] private List<GameLevel> gameLevelList;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private int score;
    private float time;
    private bool isTimerActive = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;

        GameInput.Instance.OnMenuButtonPressed += GameInput_OnMenuButtonPressed;

        LoadCurrentLevel();
    }

    private void GameInput_OnMenuButtonPressed(object sender, EventArgs e)
    {
        PauseUnpauseGame();
    }

    private void Update()
    {
        if (isTimerActive)
        {
            time += Time.deltaTime;
        }
    }

    private void LoadCurrentLevel()
    {
        GameLevel level = GetGameLevel();
        GameLevel spawnGameLevel = Instantiate(level, Vector3.zero, Quaternion.identity);
        Lander.Instance.transform.position = spawnGameLevel.GetLanderStartPosition();

        cinemachineCamera.Target.TrackingTarget = spawnGameLevel.GetCameraStartTarget();
        CinemachineCameraZoom2D.Instance.SetOrthographicSize(level.GetCameraLensSize());
    }

    private GameLevel GetGameLevel()
    {
        foreach (GameLevel level in gameLevelList)
        {
            if (level.GetLevelNumber() == levelNumber)
            {
                return level;
            }
        }
        return null;
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isTimerActive = e.state == Lander.State.Normal;

        if (e.state == Lander.State.Normal)
        {
            cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            CinemachineCameraZoom2D.Instance.SetNormalSize();
        }
    }

    private void Lander_OnCoinPickup(object sender, Lander.OnCoinPickupEventArgs e)
    {
        AddScore(e.addAmount);
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    public int GetTotalScore() => totalScore;

    public void AddScore(int addScoreAmount)
    {
        score += addScoreAmount;
        totalScore += score;
        Debug.Log(score);
    }

    public void GoToNextLevel()
    {
        levelNumber++;

        if (GetGameLevel() == null)
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        }
        else
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        }
    }

    public void RetryLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }

    private void PauseUnpauseGame()
    {
        if (Time.timeScale == 1f)
        {
            PauseGame();
        }
        else
        {
            UnpauseGame();
        }
    }

    public void PauseGame()
    {
        if (Lander.Instance.GetCurrentState() == Lander.State.GameOver) return;

        Time.timeScale = 0f;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        OnGameUnPaused?.Invoke(this, EventArgs.Empty);
    }

    public int GetLevelNumber() => levelNumber;

    public int GetScore() => score;

    public float GetTime() => time;
}
