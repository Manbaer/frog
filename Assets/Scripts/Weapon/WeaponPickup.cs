using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private bool isPickedUp = false; // Flag to track if the weapon is picked up by the player
    private Vector3 originalScale; // Variable to store the original scale of the weapon

    private void Start()
    {
        // Disable the WeaponShooting and WeaponAiming scripts initially
        DisableWeaponFunctionality();

        // Store the original scale of the weapon
        originalScale = transform.localScale;

        // Scale the weapon to 3 times its normal size
        transform.localScale = originalScale * 3f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPickedUp)
        {
            // Enable the WeaponShooting and WeaponAiming scripts when the player touches the weapon
            EnableWeaponFunctionality();

            // Notify the player's WeaponManager to switch to this weapon
            other.GetComponent<WeaponManager>().SwitchWeapon(gameObject);

            // Mark the weapon as picked up
            isPickedUp = true;

            // Reset the scale of the weapon to its original size
            transform.localScale = originalScale;
        }
    }

    private void DisableWeaponFunctionality()
    {
        // Disable the WeaponShooting and WeaponAiming scripts
        GetComponent<WeaponShooting>().enabled = false;
        GetComponent<WeaponAiming>().enabled = false;
    }

    private void EnableWeaponFunctionality()
    {
        // Enable the WeaponShooting and WeaponAiming scripts
        GetComponent<WeaponShooting>().enabled = true;
        GetComponent<WeaponAiming>().enabled = true;
    }
}
