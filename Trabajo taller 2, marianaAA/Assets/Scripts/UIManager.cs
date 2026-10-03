using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Con la I mayúscula para que coincida con FinishGoal y PlayerHealth
    public static UIManager Instance;

    [Header("Elementos de UI")]
    public TextMeshProUGUI healthText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1f;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    public void UpdateHealthUI(int currentHealth)
    {
        if (healthText != null)
        {
            healthText.text = "Vida: " + currentHealth;
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowVictory()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [Header("Puntuación")]
    public TextMeshProUGUI scoreText;
    private int currentScore = 0;

    public void AddScore(int amount)
    {
        currentScore += amount;
        if (scoreText != null)
        {
            scoreText.text = "Puntos: " + currentScore;
        }
    }
}