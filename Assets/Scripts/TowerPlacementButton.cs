using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerPlacementButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [NonSerialized] public RectTransform towerMovementPane;
    [NonSerialized] public UnityEvent onPlace = new UnityEvent();

    [SerializeField] private RectTransform towerIcon;

    private GameObject towerPrefab;
    private Button button;
    private bool isMouseOver;
    private bool isDragging;
    private CanvasScaler scaler;
    private new Camera camera;

    public void Initialize(GameObject towerPrefab)
    {
        this.towerPrefab = towerPrefab;
        Sprite sprite = towerPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
        towerIcon.GetComponent<Image>().sprite = sprite;
    }

    private void Awake()
    {
        camera = Camera.main;
        scaler = GetComponentInParent<CanvasScaler>();
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isMouseOver)
            {
                isDragging = true;
                towerIcon.SetParent(towerMovementPane, true);
            }
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            towerIcon.SetParent(transform, true);
            towerIcon.anchorMin = new Vector2(0.5f, 0.5f);
            towerIcon.anchorMax = new Vector2(0.5f, 0.5f);
            towerIcon.anchoredPosition = Vector2.zero;
            ReleaseTower(camera.ScreenToWorldPoint(Input.mousePosition));
        }

        if (isDragging)
        {
            Vector2 pos = new Vector2(Input.mousePosition.x / Screen.width, 
                Input.mousePosition.y / Screen.height);
            towerIcon.anchorMin = pos;
            towerIcon.anchorMax = pos;
            towerIcon.anchoredPosition = Vector2.zero;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        TowerTooltipController.instance.View(towerPrefab.GetComponent<Tower>());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        TowerTooltipController.instance.StopViewing(towerPrefab.GetComponent<Tower>());
    }

    public void ReleaseTower(Vector3 worldPos)
    {
        if (CameraController.instance.IsPosOutOfBounds(worldPos) ||
            TowerController.instance.IsTowerPlacedAtWorldPosition(worldPos))
            return;
        WaveController.instance.StartGame();
        TowerController.instance.PlaceTowerAtWorldPosition(towerPrefab, worldPos);
        onPlace.Invoke();
    }
}