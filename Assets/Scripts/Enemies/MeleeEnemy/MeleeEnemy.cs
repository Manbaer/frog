using System.IO;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public float speed = 3f; // Movement speed of the enemy
    public SpriteRenderer spriteRenderer; // Reference to the sprite renderer component

    private Transform player; // Reference to the player's position

    void Start()
    {
        // Find the player object by tag ("Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // Move towards the player's position
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // Calculate the direction to the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Check the horizontal direction of movement
        if (direction.x < 0)
        {
            // Moving left, flip the sprite horizontally
            spriteRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            // Moving right, flip the sprite back to normal
            spriteRenderer.flipX = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the enemy collides with the player
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("eat my balsz");
            Application.Quit();
        }
    }
}