using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject speedUpArrowGameObject;
    [SerializeField] private GameObject speedDownArrowGameObject;
    [SerializeField] private GameObject speedLeftArrowGameObject;
    [SerializeField] private GameObject speedRightArrowGameObject;
    [SerializeField] private Image fuelBarImage;

    private void Start()
    {
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;

        Hide();
    }

    private void Update()
    {
        UpdateStatsTextMesh();
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        if (e.state == Lander.State.Normal)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void UpdateStatsTextMesh()
    {
        speedUpArrowGameObject.SetActive(Lander.Instance.GetSpeedY() >= 0f);
        speedDownArrowGameObject.SetActive(Lander.Instance.GetSpeedY() < 0f);

        speedLeftArrowGameObject.SetActive(Lander.Instance.GetSpeedX() < 0f);
        speedRightArrowGameObject.SetActive(Lander.Instance.GetSpeedX() >= 0f);

        fuelBarImage.fillAmount = Lander.Instance.GetFuelAmountNormalized();

        if (fuelBarImage.fillAmount >= 0.65)
        {
            fuelBarImage.color = Color.green;
        }
        else if (fuelBarImage.fillAmount >= 0.35)
        {
            fuelBarImage.color = Color.yellow;
        }
        else
        {
            fuelBarImage.color = Color.red;
        }

        statsTextMesh.text =
            GameManager.Instance.GetLevelNumber() + "\n" +
            GameManager.Instance.GetScore() + "\n" +
            Mathf.Round(GameManager.Instance.GetTime()) + "\n" +
            Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedX() * 10f)) + "\n" +
            Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedY() * 10f));
    }

    private void Show() => gameObject.SetActive(true);

    private void Hide() => gameObject.SetActive(false);
}
