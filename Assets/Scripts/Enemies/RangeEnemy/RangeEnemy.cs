using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab for the bullet
    public float shootingInterval = 1.5f; // Interval between shots
    public float bulletSpeed = 5f; // Speed of the bullet
    public float despawnTime = 5f; // Time before the bullet despawns

    private Transform player; // Reference to the player's transform
    private float timer = 0f; // Timer for shooting interval

    void Start()
    {
        // Find the player GameObject and get its transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Rotate the enemy to face the player
        Vector2 direction = player.position - transform.position;

        // Determine if the player is to the left or right of the enemy
        if (direction.x < 0)
        {
            // Player is to the left, flip the enemy (negative scale on X axis)
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // Player is to the right, normal scale on X axis
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Update the shooting timer
        timer += Time.deltaTime;
        if (timer >= shootingInterval)
        {
            Shoot();
            timer = 0f; // Reset the timer
        }
    }

    void Shoot()
    {
        // Instantiate a bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Calculate the direction to the player
        Vector2 direction = player.position - transform.position;

        // Rotate the bullet to face the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Get the rigidbody of the bullet and apply velocity towards the player
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * bulletSpeed;

        // Destroy the bullet after despawnTime seconds
        Destroy(bullet, despawnTime);
    }
}
