using UnityEngine;
using UnityEngine.UI;

public class TowerGenerationController : MonoBehaviour
{
    public RectTransform towerMovementPane;
    public RectTransform towerButtonContainer;
    
    public GameObject towerButtonPrefab;
    public int maxStock = 5;
    public float timeBetweenStocks = 3;
    public int numButtons = 3;

    public StatusDisplay statusDisplay;

    public GameObject[] towerPrefabs;

    private int stock;
    private float timeUntilNextStock;

    public void Update()
    {
        if (stock < maxStock) timeUntilNextStock -= Time.deltaTime;
        if (timeUntilNextStock <= 0)
        {
            timeUntilNextStock += timeBetweenStocks;
            AddStock();
        }
        
        statusDisplay.SetStatus($"{stock}/{maxStock} Stock", -(timeUntilNextStock / timeBetweenStocks-1));
    }

    public void AddStock()
    {
        if (stock == 0)
        {
            CreateTowerButtons();
        }

        stock++;
        if (stock > maxStock)
            stock = maxStock;
    }

    public void TowerPlaced()
    {
        stock--;
        DeleteTowerButtons();
        if (stock >= 1) CreateTowerButtons();
    }

    private void DeleteTowerButtons()
    {
        for (int i = towerButtonContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(towerButtonContainer.GetChild(i).gameObject);
        }
    }

    public void CreateTowerButtons()
    {
        GameObject[] towers = GetRandomTowerPrefabs();

        for (int i = 0; i < numButtons; i++)
        {
            TowerPlacementButton towerButton = Instantiate(towerButtonPrefab, towerButtonContainer).GetComponent<TowerPlacementButton>();
            towerButton.Initialize(towers[i]);
            towerButton.towerMovementPane = towerMovementPane;
            towerButton.onPlace.AddListener(TowerPlaced);
        }
    }

    private GameObject[] GetRandomTowerPrefabs()
    {
        int numTowers = 0;
        int[] towers = new int[numButtons];
        for (int i = 0; i < towers.Length; i++)
        {
            towers[i] = -1;
        }

        while (numTowers < numButtons)
        {
            int index = Random.Range(0, towerPrefabs.Length);
            bool exists = false;
            foreach (int existingTowerIndex in towers)
            {
                if (existingTowerIndex == index)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                towers[numTowers] = index;
                numTowers++;
            }
        }

        GameObject[] randomTowers = new GameObject[numButtons];
        for (int i = 0; i < numButtons; i++)
        {
            randomTowers[i] = towerPrefabs[towers[i]];
        }

        return randomTowers;
    }
}
