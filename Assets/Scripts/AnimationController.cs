using UnityEngine;

public class AnimationController : MonoBehaviour
{   
    
    Animator animator;
    
    void Start()
    {
        
        animator = GetComponent<Animator>();
        if (animator == null)
        
        {
           Debug.LogError("Animator component not found on this game object.");
           enabled = false;
           return;
        }
    }

    void Update()
    {
        UpdateAnimationParameters();

    }

    private void UpdateAnimationParameters()
    {
        bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        animator.SetBool("isMoving", isMoving); 

        if (Input.GetKey(KeyCode.LeftShift))
        {
        animator.SetBool("isRunning", true); 
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
        animator.SetBool("isRunning", false);    
        }
    }
}
