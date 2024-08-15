using UnityEngine;

public class lfoSwitch : MonoBehaviour
{
    public Animator animator;  // Reference to the Animator component
    private bool isOn = false; // Tracks the current state of the switch

    void Start()
    {
        // Ensure the Animator component is assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found!");
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform raycast to check if the GameObject is clicked
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject) // Check if the hit object is this GameObject
                {
                    ToggleSwitch();
                }
            }
        }
    }

    private void ToggleSwitch()
    {
        isOn = !isOn; // Toggle the switch state
        animator.SetTrigger("ToggleSwitch");
        Debug.Log($"Switch is now {(isOn ? "On" : "Off")}");
    }
}
