using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerController : MonoBehaviour
{
    public static TowerController instance;
    
    public Tilemap towerTilemap;
    public Tile blankTile;

    private void Awake()
    {
        instance = this;
    }

    public bool IsTowerPlacedAtWorldPosition(Vector3 position)
    {
        return IsTowerPlacedAtPosition(towerTilemap.WorldToCell(position));
    }
    public bool IsTowerPlacedAtPosition(Vector3Int position)
    {
        return towerTilemap.GetTile(position) != null;
    }

    public void PlaceTowerAtWorldPosition(GameObject towerPrefab, Vector3 position)
    {
        PlaceTowerAtPosition(towerPrefab, new Vector3Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y)));
    }
    public void PlaceTowerAtPosition(GameObject towerPrefab, Vector3Int position)
    {
        Instantiate(towerPrefab, position + new Vector3(0.5f, 0.5f), Quaternion.identity, transform);
        towerTilemap.SetTile(position, blankTile);
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}