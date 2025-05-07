using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Animator anim; 

    private void Start()
    {
        // Initialize player health through PlayerStats
        if (PlayerStats.Instance.Health == 0)
        {
            // If PlayerStats is not initialized properly, set initial health
            PlayerStats.Instance.Heal(PlayerStats.Instance.MaxHealth); // Set to max health
        }

        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component is missing from the player!");
        }
    }

    // Function to handle taking damage
    public void TakeDamage(int damageAmount)
    {
        // Call PlayerStats to reduce health
        PlayerStats.Instance.TakeDamage(damageAmount);
        Debug.Log("Player took damage. Current health: " + PlayerStats.Instance.Health);

        if (PlayerStats.Instance.Health <= 0)
        {
            Die();
        }
    }

    // Function to handle player death (same as before, but using PlayerStats)
    private void Die()
    {
        Debug.Log("Player is dead!");

        // Trigger the death animation
        if (anim != null)
        {
            anim.SetTrigger("Death"); // Make sure "Death" trigger exists in the Animator
        }

        // Disable the player after death
        gameObject.SetActive(false);
    }

    // Optional: Function to heal the player
    public void Heal(int healAmount)
    {
        // Heal the player through PlayerStats
        PlayerStats.Instance.Heal(healAmount);
        Debug.Log("Player healed. Current health: " + PlayerStats.Instance.Health);
    }
}
