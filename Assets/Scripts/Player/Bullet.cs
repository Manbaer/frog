using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1; // Amount of damage the bullet deals

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet collides with an enemy
        if (other.CompareTag("Enemy"))
        {
            // Get a reference to the enemy's health indicator script
            EnemyHealthIndicator enemyHealth = other.GetComponent<EnemyHealthIndicator>();

            // Check if the enemy has a health indicator script
            if (enemyHealth != null)
            {
                // Apply damage to the enemy
                enemyHealth.TakeDamage(damage);
            }

            // Destroy the bullet upon collision with an enemy
            Destroy(gameObject);
        }
    }
}
