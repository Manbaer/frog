using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    public string weaponTag = "Weapon"; // Tag of the weapons
    public string playerTag = "Player"; // Tag of the player
    public Text ammoText; // Reference to the Text component for displaying ammo reserve count

    void FixedUpdate()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag); // Find the player object

        if (player != null && ammoText != null)
        {
            GameObject[] weapons = GameObject.FindGameObjectsWithTag(weaponTag); // Find all objects with the specified tag

            // Check if any weapons were found
            if (weapons.Length > 0)
            {
                // Find the first weapon with the WeaponShooting script
                foreach (GameObject weapon in weapons)
                {
                    WeaponShooting weaponScript = weapon.GetComponent<WeaponShooting>();
                    if (weaponScript != null)
                    {
                        // Get ammo count from the weapon script
                        int ammoCount = weaponScript.currentBullets;
                        ammoText.text = ammoCount.ToString();

                        // Align the text above the player's head
                        Vector3 offset = new Vector3(0f, 0.3f, 0f); // Adjust the offset as needed
                        ammoText.transform.position = player.transform.position + offset;
                        ammoText.transform.rotation = Quaternion.LookRotation(ammoText.transform.position - Camera.main.transform.position);

                        // Stop searching after finding the first weapon
                        return;
                    }
                }
            }
        }

        // Hide or disable the ammo display if no player or weapons are found
        if (ammoText != null)
        {
            ammoText.gameObject.SetActive(false);
        }
    }
}
