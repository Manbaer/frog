using UnityEngine;
using System.Collections;

public class WeaponShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Reference to the projectile prefab
    public GameObject muzzleFlashPrefab; // Reference to the muzzle flash prefab
    public GameObject bulletShellPrefab; // Reference to the bullet shell prefab
    public Transform bulletShellSpawnPoint; // Reference to the point where bullet shells will be spawned
    public Transform gun;
    public float bulletSpeed = 10f; // Speed of the projectile
    public int maxBullets = 8; // Maximum number of bullets the weapon can hold
    public float reloadTime = 0.5f; // Time it takes to reload in seconds
    public float muzzleFlashDuration = 0.1f; // Duration of the muzzle flash
    public float shellUpwardSpeed = 2f; // Speed at which the bullet shell moves upward
    public float shellDespawnDelay = 0.3f; // Delay before despawning the bullet shell
    public float bulletDespawnDelay = 2f; // Delay before despawning the bullet
    public float hoverHeight = 0.5f; // Hover height radius
    public float fireRate = 0.1f; // Rate of fire (bullets per second)
    public bool continuousFire = false; // Allow continuous firing while the fire button is held down
    public AudioSource reloadSound; //reload audio

    private int currentBullets; // Number of bullets the player currently has
    private bool isReloading; // Flag to track if the player is currently reloading
    private bool isFiring; // Flag to track if the weapon is currently firing
    private Coroutine fireCoroutine; // Coroutine reference for continuous firing
    private Transform firePoint; // Reference to the point where projectiles will be spawned

    void Start()
    {
        currentBullets = maxBullets; // Initialize the current bullets count
        // Get the fire point from a child object
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("FirePoint not found as a child object.");
        }
    }

    void Update()
    {
        // Calculate the distance between the player's position and the mouse position
        float distanceToMouse = Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

        // Check if the player presses the fire button (e.g., left mouse button) and if not currently reloading
        if ((Input.GetButtonDown("Fire1") && !continuousFire && !isReloading && currentBullets > 0 && distanceToMouse > hoverHeight) ||
            (continuousFire && Input.GetButton("Fire1") && !isReloading && currentBullets > 0 && distanceToMouse > hoverHeight && !isFiring))
        {
            // Start firing a single shot or continuous fire based on the continuousFire flag
            if (continuousFire)
            {
                StartContinuousFire();
            }
            else
            {
                Shoot();
            }
        }

        // Check if the player releases the fire button
        if (Input.GetButtonUp("Fire1"))
        {
            // Stop continuous firing if it's active
            if (continuousFire)
            {
                StopContinuousFire();
            }
        }

        // Check if the player presses the reload button (e.g., R) and if not currently reloading
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            Reload(); // Call the Reload method
        }
    }

    void Shoot()
    {
        // Calculate the direction from the fire point to the mouse position
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position).normalized;

        // Set a minimum bullet speed
        float minBulletSpeed = 5f; // Adjust as needed

        // Calculate the velocity magnitude based on the bullet speed and the minimum bullet speed
        float velocityMagnitude = Mathf.Max(bulletSpeed, minBulletSpeed);

        // Set the velocity magnitude of the direction vector
        direction *= velocityMagnitude;

        // Calculate the angle of rotation from the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Instantiate muzzle flash effect at the fire point position
        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, firePoint.position, Quaternion.identity);

        // Flip the muzzle flash on the y-axis if the gun is facing left
        if (gun.localScale.x < 0)
        {
            Vector3 scale = muzzleFlash.transform.localScale;
            scale.y *= -1;
            muzzleFlash.transform.localScale = scale;
        }

        // Rotate the muzzle flash to face the calculated direction
        muzzleFlash.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Destroy the muzzle flash after the specified duration
        Destroy(muzzleFlash, muzzleFlashDuration);

        // Instantiate bullet shell effect at the bullet shell spawn point
        GameObject shell = Instantiate(bulletShellPrefab, bulletShellSpawnPoint.position, Quaternion.identity);

        // Adjust the position of the bullet shell
        shell.transform.position = new Vector3(shell.transform.position.x, shell.transform.position.y, -1);

        // Get the Rigidbody2D component of the bullet shell
        Rigidbody2D shellRb = shell.GetComponent<Rigidbody2D>();

        // Set the initial upward velocity of the bullet shell
        if (shellRb != null)
        {
            shellRb.velocity = Vector2.up * shellUpwardSpeed;

            // Despawn the bullet shell after the specified delay
            Destroy(shell, shellDespawnDelay);
        }

        // Instantiate a new bullet at the fire point position and rotation
        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.AngleAxis(angle, Vector3.forward));

        // Flip the bullet sprite on the x-axis if the gun is facing left
        if (gun.localScale.x < 0)
        {
            Vector3 bulletScale = newBullet.transform.localScale;
            bulletScale.x *= -1;
            newBullet.transform.localScale = bulletScale;
            angle += 180f; // Adjust the angle to ensure correct rotation
        }

        // Rotate the bullet to face the correct direction
        newBullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // If the bullet has a Rigidbody2D component, set its velocity based on the direction vector
        Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            // Set the velocity of the bullet
            bulletRb.velocity = direction;

            // Despawn the bullet after the specified delay
            Destroy(newBullet, bulletDespawnDelay);

            currentBullets--; // Decrease the number of bullets
        }

        // Check if the player is out of bullets
        if (currentBullets <= 0)
        {
            // Start reloading
            Reload();
        }
    }

    void Reload()
    {
        isReloading = true; // Set the reloading flag

        if (reloadSound != null)
        {
            reloadSound.Play();
        }

        // Start a coroutine to wait for the reload time
        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime); // Wait for the reload time

        // Reset the current bullets count to the maximum
        currentBullets = maxBullets;

        isReloading = false; // Reset the reloading flag

        // Resume continuous firing if it was active before reloading
        if (continuousFire && isFiring)
        {
            StartContinuousFire();
        }
    }

    void StartContinuousFire()
    {
        if (!isFiring)
        {
            // Start firing routine
            fireCoroutine = StartCoroutine(FireRoutine());
            isFiring = true;
        }
    }

    void StopContinuousFire()
    {
        if (isFiring)
        {
            // Stop firing routine
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
            }
            isFiring = false;
        }
    }

    IEnumerator FireRoutine()
    {
        while (currentBullets > 0 && !isReloading)
        {
            Shoot(); // Call the Shoot method
            yield return new WaitForSeconds(1f / fireRate); // Wait for the specified fire rate
        }
        isFiring = false; // Reset the firing flag when done
    }
}
