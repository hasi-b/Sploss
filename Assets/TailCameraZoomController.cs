using UnityEngine;
using Cinemachine;

public class TailCameraZoomController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public TailManager tailManager; // Drag your TailManager object here
    public float zoomStep = 1f;
    public float zoomSpeed = 2f;

    private int lastTailCount = 0;
    private float targetZoom;

    void Start()
    {
        if (virtualCamera == null)
            Debug.LogError("Virtual Camera not assigned!");

        if (tailManager == null)
            tailManager = FindObjectOfType<TailManager>();

        targetZoom = virtualCamera.m_Lens.OrthographicSize;
    }

    void Update()
    {
        int currentTailCount = tailManager.TailObjectCount;

        if (currentTailCount > lastTailCount)
        {
            targetZoom += zoomStep;
            lastTailCount = currentTailCount;
        }

        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
            virtualCamera.m_Lens.OrthographicSize,
            targetZoom,
            Time.deltaTime * zoomSpeed
        );
    }
}