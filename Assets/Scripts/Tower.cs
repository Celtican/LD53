using System;
using UnityEditor;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public string towerName = "New Tower";
    [TextArea(2, 4)] public string towerDescription = "";
    public TowerType towerType = TowerType.Modifier;
    public Rarity rarity = Rarity.Common;

    private void OnMouseEnter()
    {
        TowerTooltipController.instance.View(this);
    }

    private void OnMouseExit()
    {
        print($"Mouse exiting {towerName}");
        TowerTooltipController.instance.StopViewing(this);
    }

    public string GetTowerType()
    {
        switch (towerType)
        {
            case TowerType.Producer:
                return "Producer";
            case TowerType.Modifier:
                return "Modifier";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public string GetRarity()
    {
        switch (rarity)
        {
            case Rarity.Common:
                return "Common";
            case Rarity.Rare:
                return "Rare";
            case Rarity.Legendary:
                return "Legendary";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public enum TowerType
    {
        Producer,
        Modifier
    }

    public enum Rarity
    {
        Common,
        Rare,
        Legendary
    }
}
