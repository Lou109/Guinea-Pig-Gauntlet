using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Adjustable in Inspector
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

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
            Debug.Log("Player died!");
            // Add death handling here (game over, restart, etc.)
        }
    }

    public int GetHealth() => currentHealth;
}

