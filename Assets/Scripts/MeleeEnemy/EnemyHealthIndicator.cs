using UnityEngine;

public class EnemyHealthIndicator : MonoBehaviour
{
    public int maxHealth = 3; // Maximum health of the enemy
    private int currentHealth; // Current health of the enemy
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer component
    public Color fullHealthColor = Color.green; // Color at full health
    public Color lowHealthColor = Color.red; // Color at low health
    public float colorTransitionSpeed = 2f; // Speed of color transition
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip hitSound; // Audio clip to play when the enemy is hit
    public AudioClip deathSound; // Audio clip to play when the enemy dies

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer component
    }

    void Update()
    {
        // Calculate the lerped color based on current health
        Color lerpedColor = Color.Lerp(lowHealthColor, fullHealthColor, (float)currentHealth / maxHealth);

        // Apply the lerped color to the sprite renderer
        spriteRenderer.color = lerpedColor;
    }

    public void TakeDamage(int damageAmount)
    {
        // Reduce current health by the damage amount
        currentHealth -= damageAmount;

        // Clamp current health to ensure it stays within the valid range
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Play hit sound
        audioSource.PlayOneShot(hitSound);

        // Check if the enemy has been defeated
        if (currentHealth <= 0)
        {
            Die(); // Call the Die method or handle enemy defeat
        }
    }

    void Die()
    {
        // Play death sound
        audioSource.PlayOneShot(deathSound);

        // Implement death behavior, such as destroying the enemy GameObject
        Destroy(gameObject);
    }
}
