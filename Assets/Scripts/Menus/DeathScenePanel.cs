using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathScenePanel : MonoBehaviour
{
    public float fadeOutDuration = 2f; // Duration for the fade-out effect
    private Image panelImage; // Reference to the Image component

    void Start()
    {
        // Get the Image component attached to this GameObject
        panelImage = GetComponent<Image>();

        // Start the fade-out animation
        FadeOut();
    }

    void FadeOut()
    {
        // Calculate the speed at which the alpha should change per second
        float alphaChangePerSecond = 1f / fadeOutDuration;

        // Use a coroutine to gradually decrease the alpha of the panel
        StartCoroutine(FadeOutCoroutine(alphaChangePerSecond));
    }

    IEnumerator FadeOutCoroutine(float alphaChangePerSecond)
    {
        // Gradually decrease the alpha value until it reaches 0 (fully transparent)
        while (panelImage.color.a > 0f)
        {
            // Calculate the new alpha value
            float newAlpha = Mathf.MoveTowards(panelImage.color.a, 0f, alphaChangePerSecond * Time.deltaTime);

            // Set the new alpha value for the Image component
            SetAlpha(newAlpha);

            yield return null; // Wait for the next frame
        }

        // If the alpha value has reached 0, destroy the GameObject
        Destroy(gameObject);
    }

    void SetAlpha(float alpha)
    {
        // Get the current color of the Image component
        Color color = panelImage.color;

        // Set the alpha value of the color
        color.a = alpha;

        // Assign the modified color back to the Image component
        panelImage.color = color;
    }
}
