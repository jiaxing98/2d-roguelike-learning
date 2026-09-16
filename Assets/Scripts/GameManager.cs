using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TurnManager TurnManager { get; private set; }
    public int FoodAmount => m_FoodAmount;
    public int FoodConsumedPerTurn = 1;

    public BoardManager BoardManager;
    public PlayerController PlayerController;
    
    public event Action<int> OnFoodChanged;
    public event Action<int> OnGameOver;
    public event Action<int> OnStartNewGame;

    private int m_CurrentLevel = 1;
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

    public void ChangeFood(int amount)
    {
        m_FoodAmount += amount;
        OnFoodChanged?.Invoke(m_FoodAmount);

        if (m_FoodAmount <= 0)
        {
            PlayerController.GameOver();
            OnGameOver?.Invoke(m_CurrentLevel);
        }
    }

    public void NewLevel()
    {
        BoardManager.Clean();
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));

        m_CurrentLevel++;
    }

    public void StartNewGame()
    {
        m_CurrentLevel = 1;
        m_FoodAmount = 20;

        BoardManager.Clean();
        BoardManager.Init();

        PlayerController.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));

        OnStartNewGame?.Invoke(m_FoodAmount);
    }

    void OnTurnHappen()
    {
        ChangeFood(-FoodConsumedPerTurn);
    }
}
