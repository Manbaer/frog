using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyHealthIndicator : MonoBehaviour
{
    public int maxHealth = 3; // Maximum health of the enemy
    private int currentHealth; // Current health of the enemy
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer component
    public Color originalColor; // Original color of the enemy
    public float darknessIncrement = 0.3f; // Amount to darken the color per hit
    public AudioClip hitAudio; // Reference to the AudioSource component for hit sound
    public AudioClip deathAudio; // Reference to the AudioSource component for death sound

    private AudioSource audioSource;
    private WaveManager waveManager; // Reference to the WaveManager script
    private float cumulativeDarkness = 0f; // Cumulative darkness value

    void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth; // Initialize current health
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer component
        originalColor = spriteRenderer.color; // Save the original color
    }

    public void TakeDamage(int damageAmount)
    {
        // Reduce current health by the damage amount
        currentHealth -= damageAmount;

        // Clamp current health to ensure it stays within the valid range
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Increase the cumulative darkness
        cumulativeDarkness += darknessIncrement * damageAmount;

        // Darken the color based on cumulative darkness
        Color newColor = new Color(
            Mathf.Max(originalColor.r - cumulativeDarkness, 0f),
            Mathf.Max(originalColor.g - cumulativeDarkness, 0f),
            Mathf.Max(originalColor.b - cumulativeDarkness, 0f),
            originalColor.a);

        // Apply the new color to the sprite renderer
        spriteRenderer.color = newColor;

        // Play hit sound
        audioSource.PlayOneShot(hitAudio);

        // Check if the enemy has been defeated
        if (currentHealth <= 0)
        {
            Die(); // Call the Die method or handle enemy defeat
        }
    }

    void Die()
    {
        if (waveManager != null)
        {
            waveManager.RangeEnemyDefeated(gameObject);
            waveManager.MeleeEnemyDefeated(gameObject);
        }

        GameObject deathSound = new GameObject("Death Sound rhah");
        AudioSource deathSrc = deathSound.AddComponent<AudioSource>();
        deathSrc.PlayOneShot(deathAudio);
        Destroy(deathSound, deathAudio.length);

        // Implement death behavior, such as destroying the enemy GameObject
        Destroy(gameObject);
    }
}
