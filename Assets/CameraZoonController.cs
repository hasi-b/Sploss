using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Cinemachine;

public class VirtualCameraZoomManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public PlayerInteraction playerInteraction; // Assign in Inspector or find in Start
    public float zoomStep = 1f;
    public float zoomSpeed = 2f;

    private int lastObjectCount = 0;
    private float targetZoom;

    void Start()
    {
        if (virtualCamera == null)
            Debug.LogError("Virtual camera not assigned!");

        if (playerInteraction == null)
            playerInteraction = FindObjectOfType<PlayerInteraction>(); // fallback

        targetZoom = virtualCamera.m_Lens.OrthographicSize;
    }

    void Update()
    {
        int currentCount = playerInteraction.AttachedObjectCount;

        if (currentCount > lastObjectCount)
        {
            targetZoom += zoomStep;
            lastObjectCount = currentCount;
        }

        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
            virtualCamera.m_Lens.OrthographicSize,
            targetZoom,
            Time.deltaTime * zoomSpeed
        );
    }
}