using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private PlayerLives playerLives;  // Assign in inspector

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"Player took {damageAmount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player health reached zero, triggering lose life.");
            if (playerLives != null)
            {
                playerLives.LoseLife();
            }
            else
            {
                Debug.LogWarning("PlayerLives reference not assigned in inspector");
            }
            currentHealth = maxHealth; // reset for next life
        }
    }

    public int GetHealth() => currentHealth;
}

