using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public GameObject spawnDeath;
    public int damage = 1; // Amount of damage the bullet deals
    public bool affectsEnemy = true; // Flag to determine if the bullet affects enemies

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet collides with an enemy and the bullet affects enemies
        if (affectsEnemy && other.CompareTag("Enemy"))
        {
            // Get a reference to the enemy's health indicator script
            EnemyHealthIndicator enemyHealth = other.GetComponent<EnemyHealthIndicator>();

            // Check if the enemy has a health indicator script
            if (enemyHealth != null)
            {
                // Apply damage to the enemy

                if (enemyHealth.currentHealth-damage <= 0)
                {
                    ScoreManager.Score += enemyHealth.killScore;
                }

                enemyHealth.TakeDamage(damage);
            }

            // Destroy the bullet upon collision with an enemy
            Destroy(gameObject);
        }
        // Check if the bullet collides with the player and the bullet affects the player
        else if (!affectsEnemy && other.CompareTag("Player"))
        {
            if (other.transform.CompareTag("Player"))
            {
                Debug.Log("eat my balsz");
                Instantiate(spawnDeath);
                Destroy(gameObject);
            }
        }
    }
}