using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTransition : MonoBehaviour
{
    public AudioSource audioSource;
    public float transitionDelay = 2f; // Delay before transitioning to death scene
    public GameObject blackPanelPrefab; // Reference to the black panel GameObject

    void Start()
    {
        // Stop all currently playing audio
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.Stop();
        }
        
        //Play death sound
        audioSource.Play();

        // Stop player movement
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Player playerMovement = player.GetComponent<Player>();
            if (playerMovement != null)
            {
                playerMovement.moveSpeed = 0;
                playerMovement.enabled = false;
            }
        }

        // Destroy all objects with the tag "Gun"
        GameObject[] guns = GameObject.FindGameObjectsWithTag("Gun");
        foreach (GameObject gun in guns)
        {
            Destroy(gun);
        }

        // Remove all enemies from the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // Remove all bullets from the scene
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }

        // Spawn the black panel prefab
        if (blackPanelPrefab != null)
        {
            Instantiate(blackPanelPrefab, Vector3.zero, Quaternion.identity);
        }


        // Start the transition to the death scene after a delay
        Invoke("TransitionToDeathScene", transitionDelay);
    }

    void TransitionToDeathScene()
    {
        // Load the death scene
        SceneManager.LoadScene("Death");
    }
}
