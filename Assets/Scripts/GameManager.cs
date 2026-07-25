using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Required for the new Input System keyboard check

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Panels / Canvases")]
    public GameObject mainMenuPanel;
    public GameObject gameplayHUD;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    private bool isGameStarted = false;
    private bool isPaused = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ShowMainMenu();
    }

    void Update()
    {
        // Only allow pausing if the game has actually started
        // Using the new Input System keyboard check:
        if (isGameStarted && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    // --- MAIN MENU FLOW ---
    public void ShowMainMenu()
    {
        Time.timeScale = 0f;
        isGameStarted = false;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (gameplayHUD != null) gameplayHUD.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        isGameStarted = true;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (gameplayHUD != null) gameplayHUD.SetActive(true);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }

    // --- PAUSE MENU FLOW ---
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pausePanel != null) pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --- GAME OVER FLOW ---
    public void TriggerGameOver()
    {
        Time.timeScale = 0f;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (gameplayHUD != null) gameplayHUD.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}