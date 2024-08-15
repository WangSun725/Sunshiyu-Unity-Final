using UnityEngine;

public class KnobRotation : MonoBehaviour
{
    public Transform[] knobParts; // Assign obj33, obj34, obj35 in the Inspector
    public float rotationSpeed = 10f; // Speed of the rotation
    private float currentRotationAngle = 0f; // Tracks the current rotation angle

    private bool isDragging = false;
    private Vector3 lastMousePosition;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object is one of the knob parts
                foreach (Transform part in knobParts)
                {
                    if (hit.collider != null && hit.collider.transform == part)
                    {
                        isDragging = true;
                        lastMousePosition = Input.mousePosition;
                        break;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            float rotationAmount = mouseDelta.x * rotationSpeed * Time.deltaTime; // Horizontal drag for rotation
            currentRotationAngle += rotationAmount; // Accumulate the rotation angle
            SetKnobRotation(currentRotationAngle);
            lastMousePosition = Input.mousePosition;
        }
    }

    void SetKnobRotation(float angle)
    {
        foreach (Transform part in knobParts)
        {
            // Preserve the original position
            Vector3 originalPosition = part.localPosition;

            // Set each part's local rotation to the new angle around the Y-axis
            part.localRotation = Quaternion.Euler(0, angle, 0);

            // Restore the original position to ensure it doesn't change
            part.localPosition = originalPosition;
        }
    }
}
