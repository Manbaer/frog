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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

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
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

        // Get the rigidbody of the bullet and apply velocity towards the player
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = (player.position - transform.position).normalized * bulletSpeed;

        // Destroy the bullet after despawnTime seconds
        Destroy(bullet, despawnTime);
    }
}
