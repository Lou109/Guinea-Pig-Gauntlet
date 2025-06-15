using UnityEngine;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;                       // Total lives
    [SerializeField] private GameObject guineaPigPrefab;             // Your guinea pig RawImage prefab
    [SerializeField] private Transform guineaPigsContainer;         // Container with HorizontalLayoutGroup
    [SerializeField] private float guineaPigSpacing = 10f;           // Spacing between icons
    [SerializeField] private RenderTexture guineaPigRenderTexture;   // Your render texture

    private RawImage[] guineaPigs;   // List of instantiated guinea pig icons
    private int currentLives;

    private void Start()
    {
        // Ensure the list length matches maxLives
        guineaPigs = new RawImage[maxLives];

        // Set the spacing in the layout group (if assigned)
        var layoutGroup = guineaPigsContainer.GetComponent<HorizontalLayoutGroup>();
        if (layoutGroup != null)
        {
            layoutGroup.spacing = guineaPigSpacing;
        }

        // Instantiate icons
        for (int i = 0; i < maxLives; i++)
        {
            Debug.Log($"Instantiating guinea pig {i}");
            GameObject clone = Instantiate(guineaPigPrefab, guineaPigsContainer);
            RawImage img = clone.GetComponent<RawImage>();
            if (img != null)
            {
                img.texture = guineaPigRenderTexture;
                guineaPigs[i] = img;
            }
            else
            {
                Debug.LogWarning("Prefab missing RawImage component");
            }
        }

        currentLives = maxLives;
        UpdateGuineaPigDisplay();
        Debug.Log($"Total guinea pigs instantiated: {guineaPigs.Length}");
    }

    // Call this method when player loses a life
    public void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;
            UpdateGuineaPigDisplay();

            if (currentLives == 0)
            {
                Debug.Log("Game Over");
                // Handle game over logic here
            }
            else
            {
                ResetPlayerPosition();
            }
        }
    }

    private void UpdateGuineaPigDisplay()
    {
        for (int i = 0; i < guineaPigs.Length; i++)
        {
            guineaPigs[i].gameObject.SetActive(i < currentLives);
        }
        Debug.Log($"Lives remaining: {currentLives}");
    }

    private void ResetPlayerPosition()
    {
        // Your respawn logic here
        Debug.Log("Reset Player Position");
    }
}
