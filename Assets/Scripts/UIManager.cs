using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private PanelRenderer m_PanelRenderer;
    private VisualElement m_GameOverPanel;
    private Label m_GameOverMessage;
    private Label m_FoodLabel;

    void OnEnable()
    {
        m_PanelRenderer = GetComponentInChildren<PanelRenderer>();

        if (m_PanelRenderer != null)
        {
            m_PanelRenderer.RegisterUIReloadCallback(OnUIReload);
        }
    }

    void Start()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnFoodChanged += UpdateFoodLabel;
        GameManager.Instance.OnGameOver += ShowGameOverPanel;
        GameManager.Instance.OnStartNewGame += StartNewGame;
        UpdateFoodLabel(GameManager.Instance.FoodAmount);
    }

    void OnDisable()
    {
        if (m_PanelRenderer != null)
        {
            m_PanelRenderer.UnregisterUIReloadCallback(OnUIReload);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnFoodChanged -= UpdateFoodLabel;
        }
    }

    void OnUIReload(PanelRenderer pr, VisualElement root, int version)
    {
        m_FoodLabel = root.Q<Label>("FoodLabel");

        m_GameOverPanel = root.Q<VisualElement>("GameOverPanel");
        m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage");
        m_GameOverPanel.style.visibility = Visibility.Hidden;

        if (GameManager.Instance != null)
        {
            UpdateFoodLabel(GameManager.Instance.FoodAmount);
        }
    }

    void UpdateFoodLabel(int amount)
    {
        if (m_FoodLabel == null) return;

        m_FoodLabel.text = "Food : " + amount;
    }

    void ShowGameOverPanel(int level)
    {
        m_GameOverPanel.style.visibility = Visibility.Visible;
        m_GameOverMessage.text = "Game Over!\n\nYou traveled through " + level + " levels";
    }

    void StartNewGame(int amount)
    {
        m_GameOverPanel.style.visibility = Visibility.Hidden;
        m_FoodLabel.text = "Food : " + amount;
    }
}
