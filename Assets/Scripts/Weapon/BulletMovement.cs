using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed = 10f; // Speed of the bullet

    void Start()
    {
        // Apply velocity to the bullet's Rigidbody component
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Move the bullet in the forward direction (right)
            rb.velocity = transform.right * speed;
        }
    }
}