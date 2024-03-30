using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of player movement
    public float dashInitialSpeed = 5f; // Initial speed of the dash
    public float dashDuration = 0.5f; // Duration of dash in seconds
    public float dashCooldown = 1f; // Cooldown period for dashing in seconds
    public AudioSource dashSound; //Audio for dashing

    private Rigidbody2D rb; // Reference to the player's Rigidbody component
    private SpriteRenderer spriteRenderer; // Reference to the player's SpriteRenderer component
    private bool isDashing = false; // Flag to track if player is currently dashing
    private float dashTimer = 0f; // Timer to track dash duration
    private float lastDashTime = 0f; // Time of the last dash
    private Vector2 dashDirection; // Direction of the dash dummy
    private float currentDashSpeed; // Current speed of the dash

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody component attached to the player GameObject
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component attached to the player GameObject
    }

    void Update()
    {
        // Check for dash input (Shift button)
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            // Check if enough time has passed since the last dash
            if (Time.time - lastDashTime >= dashCooldown)
            {
                // Perform dash
                Dash();
                lastDashTime = Time.time; // Update last dash time
            }
        }

        if (isDashing)
        {
            // Update dash timer
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashDuration)
            {
                // End dash after dash duration
                isDashing = false;
                dashTimer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        // Get input from horizontal and vertical axes
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // Calculate movement direction based on input
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // Normalize movement vector to ensure consistent speed regardless of direction
        movement.Normalize();

        if (!isDashing)
        {
            // Apply regular movement if not dashing
            rb.velocity = movement * moveSpeed;

            // Flip the character sprite horizontally if moving left
            if (moveHorizontal < 0)
            {
                spriteRenderer.flipX = true;
            }
            // Otherwise, flip it back to the original orientation
            else if (moveHorizontal > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            // Decrease in speed of dash
            currentDashSpeed = Mathf.Lerp(dashInitialSpeed, 0f, dashTimer / dashDuration);

            // Apply dash speed for a certain duration
            rb.velocity = movement * moveSpeed * currentDashSpeed;
        }
    }

    void Dash()
    {
        //Audio
        if (dashSound != null)
        {
            dashSound.Play();
        }
        // Start dashing
        isDashing = true;
        dashDirection = rb.velocity.normalized;
        dashTimer = 0f;
    }
}