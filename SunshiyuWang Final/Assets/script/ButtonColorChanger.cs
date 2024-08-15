using UnityEngine;

public class ButtonMaterialChanger : MonoBehaviour
{
    public Material originalMaterial; // The original material of the button
    public Material clickMaterial;    // The material to change to when clicked

    private Renderer buttonRenderer;

    void Start()
    {
        // Get the Renderer component of the button
        buttonRenderer = GetComponent<Renderer>();

        // Ensure the original material is set initially
        if (originalMaterial != null)
        {
            buttonRenderer.material = originalMaterial;
        }
        else
        {
            Debug.LogError("Original material not assigned.");
        }
    }

    void Update()
    {
        CheckMouseClick();
    }

    void CheckMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (Input.GetMouseButtonDown(0)) // Left mouse button
                {
                    ChangeMaterial(clickMaterial);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    ChangeMaterial(originalMaterial);
                }
            }
        }
    }

    void ChangeMaterial(Material newMaterial)
    {
        if (buttonRenderer != null && newMaterial != null)
        {
            buttonRenderer.material = newMaterial;
        }
    }
}
