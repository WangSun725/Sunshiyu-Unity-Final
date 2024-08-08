using UnityEngine;

public class CubePositionToggle : MonoBehaviour
{
    public float initialYPosition = 0f; // Initial Y position
    public float clickedYPosition = 2f; // Y position when clicked

    private static CubePositionToggle currentlyActiveCube; // Tracks the currently active cube

    private void Start()
    {
        // Set the cube's initial position
        Vector3 position = transform.position;
        position.y = initialYPosition;
        transform.position = position;
    }

    private void Update()
    {
        CheckMouseClick();
    }

    private void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject) // Check if the hit object is this cube
                {
                    TogglePosition();
                }
            }
        }
    }

    private void TogglePosition()
    {
        if (currentlyActiveCube != null)
        {
            // Reset the previously active cube to its initial position
            currentlyActiveCube.ResetPosition();
        }

        if (currentlyActiveCube == this)
        {
            // If the current cube was already active, toggle it back to initial position
            currentlyActiveCube = null;
            ResetPosition();
        }
        else
        {
            // Activate this cube
            currentlyActiveCube = this;
            SetClickedPosition();
        }
    }

    private void ResetPosition()
    {
        Vector3 position = transform.position;
        position.y = initialYPosition;
        transform.position = position;
    }

    private void SetClickedPosition()
    {
        Vector3 position = transform.position;
        position.y = clickedYPosition;
        transform.position = position;
    }
}
