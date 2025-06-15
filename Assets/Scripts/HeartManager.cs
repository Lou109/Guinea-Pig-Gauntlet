using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class HeartManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private RawImage heartPrefab;
    [SerializeField] private int numberOfHearts = 10;
    [SerializeField] private int healthPerHeart = 10;
    [SerializeField] private Transform heartsContainer;
    [SerializeField] private float heartSpacing = 50f;
    

    [SerializeField] private List<RawImage> hearts = new List<RawImage>();

    [SerializeField] private UnityEvent onGameOver; // Add this

    void Start()
    {
        SpawnHearts();
    }

    void Update()
    {
        UpdateHearts();     
    }

    void SpawnHearts()
    {
        for (int i = 0; i < numberOfHearts; i++)
        {
            RawImage newHeart = Instantiate(heartPrefab);
            hearts.Add(newHeart);
            newHeart.transform.SetParent(heartsContainer, false);

            RectTransform heartRect = newHeart.GetComponent<RectTransform>();
            heartRect.anchoredPosition = new Vector2(i * heartSpacing, 0f);
        }
    }

    void UpdateHearts()
    {
        int currentHealth = playerHealth.GetHealth();
        bool allHeartsDisabled = true;

        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth / healthPerHeart)
            {
                hearts[i].enabled = true;
                allHeartsDisabled = false; // At least one heart is enabled
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        if (allHeartsDisabled)
        {
            onGameOver.Invoke(); // Trigger game over event
        }
    }
}

