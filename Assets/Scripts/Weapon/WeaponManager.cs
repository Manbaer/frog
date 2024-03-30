using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private GameObject currentWeapon; // Reference to the currently equipped weapon

    public void SwitchWeapon(GameObject newWeapon)
    {
        // If the player already has a weapon, destroy it
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // Equip the new weapon
        currentWeapon = newWeapon;
    }
}
