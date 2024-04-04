using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DeathManager : MonoBehaviour
{
    public GameObject deathScreen; // Reference to the death screen panel
    public AudioSource deathSound; // Reference to the death sound
    public Button tryAgainButton; // Reference to the try again button
    public Button giveUpButton; // Reference to the give up button
    public float fadeDuration = 1f; // Duration for fading in/out

    private void Start()
    {
        // Ensure that the death screen is initially hidden
        deathScreen.SetActive(false);

        // Disable the buttons initially
        tryAgainButton.interactable = false;
        giveUpButton.interactable = false;
    }

    public void PlayerDied()
    {
        // Play the death sound
        if (deathSound != null)
        {
            deathSound.Play();
        }

        // Show the death screen
        deathScreen.SetActive(true);

        // Start fading in the buttons
        StartCoroutine(FadeButtons());
    }

    private IEnumerator FadeButtons()
    {
        // Wait for a short delay before fading in the buttons
        yield return new WaitForSeconds(0.5f);

        // Enable the buttons
        tryAgainButton.interactable = true;
        giveUpButton.interactable = true;

        // Calculate the step for fading in based on the fade duration
        float step = Time.deltaTime / fadeDuration;
        float t = 0f;

        // Fade in the buttons
        while (t < 1f)
        {
            t += step;
            Color tryAgainColor = tryAgainButton.image.color;
            Color giveUpColor = giveUpButton.image.color;
            tryAgainColor.a = Mathf.Lerp(0f, 1f, t);
            giveUpColor.a = Mathf.Lerp(0f, 1f, t);
            tryAgainButton.image.color = tryAgainColor;
            giveUpButton.image.color = giveUpColor;
            yield return null;
        }
    }

    // Method for restarting the game (called by the "Try Again" button)
    public void RestartGame()
    {
        // You can restart the game here (e.g., reload the current scene)
        // Example: SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Method for quitting the game (called by the "Give Up" button)
    public void QuitGame()
    {
        // You can quit the game here (e.g., quit application)
        // Example: Application.Quit();
    }
}
