using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private TextMeshProUGUI nextButtonTextMesh;

    [Space]
    [SerializeField] private Button nextButton;

    private Action nextButtonClickAction;

    private void Awake()
    {
        nextButton.onClick.AddListener(() =>
        {
            nextButtonClickAction();
        });
    }

    private void Start()
    {
        Lander.Instance.OnLanded += Lander_OnLanded;

        Hide();
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.Success:
                titleTextMesh.text = "SUCCESSFUL LANDING!";
                titleTextMesh.color = Color.green;
                nextButtonTextMesh.text = "CONTINUE";
                nextButtonClickAction = () =>
                {
                    GameManager.Instance.GoToNextLevel();
                };
                break;
            default:
                titleTextMesh.text = "CRASH LANDED!";
                titleTextMesh.color = Color.red;
                nextButtonTextMesh.text = "RETRY";
                nextButtonClickAction = () =>
                {
                    GameManager.Instance.RetryLevel();
                };
                break;
        }

        statsTextMesh.text =
            Mathf.Round(e.landingSpeed * 2f) + "\n" +
            Mathf.Round(e.dotVector * 100) + "\n" +
            'x' + e.scoreMultiplier + "\n" +
            e.score + "\n";

        Show();
    }

    private void Show() => gameObject.SetActive(true);

    private void Hide() => gameObject.SetActive(false);
}
