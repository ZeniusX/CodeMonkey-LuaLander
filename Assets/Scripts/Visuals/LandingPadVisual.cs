using TMPro;
using UnityEngine;

public class LandingPadVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro multiplierText;

    private void Awake()
    {
        LandingPad landingPad = GetComponent<LandingPad>();
        multiplierText.text = $"x{landingPad.GetScoreMultiplier()}";
    }
}
