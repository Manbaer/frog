using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyHealthIndicator : MonoBehaviour
{
    public int maxHealth = 3; // Maximum health of the enemy
    private int currentHealth; // Current health of the enemy
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer component
    public Color fullHealthColor = Color.green; // Color at full health
    public Color lowHealthColor = Color.red; // Color at low health
    public float colorTransitionSpeed = 2f; // Speed of color transition
    public AudioClip hitAudio; // Reference to the AudioSource component for hit sound
    public AudioClip deathAudio; // Reference to the AudioSource component for death sound

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        audioSource.PlayOneShot(hitAudio);

        // Check if the enemy has been defeated
        if (currentHealth <= 0)
        {
            Die(); // Call the Die method or handle enemy defeat
        }
    }

    void Die()
    {

        GameObject deathSound = new GameObject("Death Sound rhah");
        AudioSource deathSrc = deathSound.AddComponent<AudioSource>();
        deathSrc.PlayOneShot(deathAudio);
        Destroy(deathSound, deathAudio.length);

        // Implement death behavior, such as destroying the enemy GameObject
        Destroy(gameObject);
    }
}