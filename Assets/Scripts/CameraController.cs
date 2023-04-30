using System;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    
    public float minimumX = 7;
    public float minimumY = 5;
    public float horizontalMovementStep = 3;
    public RectTransform hud;
    
    private Camera mainCamera;
    private Canvas canvas;
    private float hudHeight = 0;
    private float xPos;

    private void Awake()
    {
        instance = this;
        canvas = hud.GetComponentInParent<Canvas>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        xPos = mainCamera.transform.position.x;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(minimumX, minimumY, 0));
    }

    private void Update()
    {
        hudHeight = GetHeightOfHUD();
        // Camera.main.orthographicSize = minimumY/2;
        // Camera.main.orthographicSize = minimumX / Camera.main.aspect / 2;
        mainCamera.orthographicSize = Mathf.Max((minimumY+hudHeight) * 0.5f, minimumX * 0.5f / mainCamera.aspect);
        mainCamera.transform.position = new Vector3(xPos, -hudHeight * 0.5f, -10);
    }

    public void MoveHorizontally()
    {
        MoveHorizontally(horizontalMovementStep);
    }
    public void MoveHorizontally(float amount)
    {
        DOTween.To(() => xPos, newPos => xPos = newPos, xPos + amount, 2);
    }

    public bool IsPosOutOfBounds(Vector2 pos)
    {
        return IsPosOutOfBounds(pos, 0);
    }
    public bool IsPosOutOfBounds(Vector2 pos, float margin)
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        cameraPosition.y += hudHeight * 0.5f;
        return pos.x < cameraPosition.x - minimumX*0.5f - margin ||
               pos.x > cameraPosition.x + minimumX*0.5f + margin ||
               pos.y < cameraPosition.y - minimumY*0.5f - margin ||
               pos.y > cameraPosition.y + minimumY*0.5f + margin;
    }

    private float GetHeightOfHUD()
    {
        float bottomPos = mainCamera.ScreenToWorldPoint(new Vector3(0, 0)).y;
        float topPos = mainCamera.ScreenToWorldPoint(new Vector3(0, hud.sizeDelta.y)).y;
        return (topPos - bottomPos) * canvas.scaleFactor;
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}
