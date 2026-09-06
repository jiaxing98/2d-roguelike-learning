using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TurnManager TurnManager { get; private set; }
    public int FoodAmount => m_FoodAmount;
    public int FoodConsumedPerTurn = 1;

    public BoardManager BoardManager;
    public PlayerController PlayerController;
    
    public event Action<int> OnFoodChanged;

    private int m_FoodAmount = 100;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen;

        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));
    }

    void OnTurnHappen()
    {
        ChangeFood(-FoodConsumedPerTurn);
    }

    public void ChangeFood(int amount)
    {
        m_FoodAmount += amount;
        OnFoodChanged?.Invoke(m_FoodAmount);
    }
}
