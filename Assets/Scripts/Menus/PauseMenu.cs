using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    void Start()
    {
        // Ensure that the pause menu is initially hidden when the game starts
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        bool isPaused = !pauseMenuUI.activeSelf;
        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f; // Pause/unpause time
    }

    public void ResumeGame()
    {
        TogglePauseMenu(); // Hide pause menu
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
        Time.timeScale = 1f; // Ensure time scale is set to normal
    }

    public void ExitGame()
    {
        Application.Quit(); // Quit the application (works in standalone builds)
    }
}
