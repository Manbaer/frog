using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DeathManager : MonoBehaviour
{
  
    public void RestartGame()
    {
        SceneManager.LoadScene("TestStage");
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }
}
