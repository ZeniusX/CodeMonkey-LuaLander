using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraZoom2D : MonoBehaviour
{
    private const float NORMAL_ORTHOGRAPHIC_SIZE = 10f;

    public static CinemachineCameraZoom2D Instance { get; private set; }

    [SerializeField] private float zoomSpeed = 2f;

    [Space]
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private float targetOrthographicSize = 10f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp
        (
            cinemachineCamera.Lens.OrthographicSize,
            targetOrthographicSize,
            Time.deltaTime * zoomSpeed
        );
    }

    public void SetOrthographicSize(float targetOrthographicSize)
    {
        this.targetOrthographicSize = targetOrthographicSize;
    }

    public void SetNormalSize()
    {
        SetOrthographicSize(NORMAL_ORTHOGRAPHIC_SIZE);
    }
}
