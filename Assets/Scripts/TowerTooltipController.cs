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
        Vector2 pos = new Vector2(Input.mousePosition.x / Screen.width, 
            Input.mousePosition.y / Screen.height);
        rectTransform.anchorMin = pos;
        rectTransform.anchorMax = pos;

        rectTransform.pivot = pos.y < 0.5f ? Vector2.zero : Vector2.up;
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    public void View(Tower tower)
    {
        string tooltip = $"><color=#00ff00> {tower.towerName}</color>.breach\n" +
                         $"<color=#90ff90>// {tower.rarity} {tower.towerType}</color>\n" +
                         $"{tower.towerDescription}";

        tooltipText.text = tooltip;
        currentlyViewing = tower;
        gameObject.SetActive(true);
    }

    public void StopViewing(Tower tower)
    {
        print($"Attempting to stop viewing {tower.towerName}");
        if (currentlyViewing != tower) return;
        print($"Stopping viewing {tower.towerName}");
        currentlyViewing = null;
        gameObject.SetActive(false);
    }
}
