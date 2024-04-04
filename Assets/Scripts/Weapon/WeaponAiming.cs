using UnityEngine;

public class WeaponAiming : MonoBehaviour
{
    public float hoverHeight = 0.5f; // Height above the player's center
    public Transform gunTransform; // Reference to the transform of the entire gun
    public bool isPaused = false;

    private Transform player; // Reference to the player's transform

    void Start()
    {
        FindPlayer(); // Call the method to find the player at the start
    }

    void Update()
    {
        if (!isPaused)
        {
            if (player == null)
            {
                FindPlayer(); // If player reference is null, find the player again
                return;
            }

            // Get the direction from the player to the mouse pointer
            Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position;
            direction.z = 0f; // Ignore the Z-axis

            // Calculate the angle between the direction vector and the right vector (1, 0)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // If the direction is to the left of the player, adjust the angle by 180 degrees
            if (direction.x < 0)
            {
                gunTransform.localScale = new Vector3(1, 1, 1);
                angle += 180f;
            }
            else
            {
                gunTransform.localScale = new Vector3(-1, 1, 1);
            }

            // Set the rotation of the gun towards the mouse pointer
            gunTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Calculate the position of the gun
            Vector3 gunPosition = player.position + direction.normalized * hoverHeight;

            // Set the position of the gun
            gunTransform.position = gunPosition;
        }
    }

    void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player"); // Find the player GameObject by tag
        if (playerObject != null)
        {
            player = playerObject.transform; // Set the player transform reference
        }
        else
        {
            Debug.LogError("Player not found in the scene!"); // Log an error if player is not found
        }
    }
}
