using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DeathManager : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI highScore;

    private void Start()
    {
        score.text = $"Score {ScoreManager.Score}";

        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (ScoreManager.Score > savedHighScore)
        {
            PlayerPrefs.SetInt("HighScore", ScoreManager.Score);
            savedHighScore = ScoreManager.Score;
        }
        highScore.text = $"High Score {savedHighScore}";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("TestStage");
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }



}
