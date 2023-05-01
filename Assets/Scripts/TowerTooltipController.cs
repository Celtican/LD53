using TMPro;
using UnityEngine;

public class TowerTooltipController : MonoBehaviour
{
    public static TowerTooltipController instance;
    private RectTransform rectTransform;
    private TMP_Text tooltipText;
    private Tower currentlyViewing;

    public void Awake()
    {
        gameObject.SetActive(false);
        instance = this;
        rectTransform = GetComponent<RectTransform>();
        tooltipText = GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector2 pos = new Vector2(Input.mousePosition.x / Screen.width,
            Input.mousePosition.y / Screen.height);
        rectTransform.anchorMin = pos;
        rectTransform.anchorMax = pos;

        Vector2 pivot = new Vector2();
        pivot.y = pos.y < 0.25f ? 0 : 1;
        pivot.x = pos.x < 0.75f ? 0 : 1;
        rectTransform.pivot = pivot;
        rectTransform.anchoredPosition = (pos.x < 0.75f ? Vector3.right : Vector3.left) * 50;
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    public void View(Tower tower)
    {
        string extension = tower.rarity == Tower.Rarity.Corporate ? ".protocol" : ".breach";
        string tooltip = $"><color=#00ff00> {tower.towerName}</color>{extension}\n" +
                         $"<color=#90ff90>// {tower.rarity} {tower.towerType}</color>\n" +
                         $"{tower.towerDescription}";

        tooltipText.text = tooltip;
        currentlyViewing = tower;
        gameObject.SetActive(true);
        UpdatePosition();
    }

    public void StopViewing(Tower tower)
    {
        if (currentlyViewing != tower) return;
        currentlyViewing = null;
        gameObject.SetActive(false);
    }
}
