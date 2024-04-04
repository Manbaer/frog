using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private GameObject[] guns; // Array to store references to active guns in the scene

    void Start()
    {
        // Ensure that the pause menu is initially hidden when the game starts
        pauseMenuUI.SetActive(false);

        // Find all active guns in the scene
        //guns = GameObject.FindGameObjectsWithTag("Gun");
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
        guns = GameObject.FindGameObjectsWithTag("Gun");
        bool isPaused = !pauseMenuUI.activeSelf;
        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f; // Pause/unpause time

        // Pause/unpause aiming and shooting on all active guns
        foreach (GameObject gun in guns)
        {
            if (gun != null)
            {
                WeaponAiming aimingScript = gun.GetComponent<WeaponAiming>();
                if (aimingScript != null)
                {
                    aimingScript.isPaused = isPaused;
                }

                WeaponShooting shootingScript = gun.GetComponent<WeaponShooting>();
                if (shootingScript != null)
                {
                    shootingScript.isPaused = isPaused;
                }
            }
        }
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
