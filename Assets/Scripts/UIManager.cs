using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private PanelRenderer m_PanelRenderer;

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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnFoodChanged += UpdateFoodLabel;
            UpdateFoodLabel(GameManager.Instance.FoodAmount);
        }
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
}
