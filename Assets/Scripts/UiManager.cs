using UnityEngine;
using TMPro; // Required for TextMeshPro

public class UIManager : MonoBehaviour
{
    // This allows other scripts to instantly find the UI Manager
    public static UIManager instance;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public GameObject[] hearts; // Drag your 3 Heart Images in here

    private int score = 0;

    void Awake()
    {
        // Set up the Singleton
        if (instance == null) instance = this;
    }

    public void AddScore()
    {
        score++;
        if (scoreText != null)
        {
            scoreText.text = "Dinos Killed: " + score;
        }
    }

    public void UpdateLives(int currentLives)
    {
        // Turn hearts on or off based on remaining lives
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentLives)
            {
                hearts[i].SetActive(true); // Heart is visible
            }
            else
            {
                hearts[i].SetActive(false); // Heart disappears!
            }
        }
    }
}