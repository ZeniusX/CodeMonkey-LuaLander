using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private Transform landerStartPositionTransform;

    [Space]
    [SerializeField] private Transform cameraStartTargetTransform;
    [SerializeField] private float cameraLensSize;

    public int GetLevelNumber() => levelNumber;

    public Vector3 GetLanderStartPosition() => landerStartPositionTransform.position;

    public Transform GetCameraStartTarget() => cameraStartTargetTransform;

    public float GetCameraLensSize() => cameraLensSize;
}
