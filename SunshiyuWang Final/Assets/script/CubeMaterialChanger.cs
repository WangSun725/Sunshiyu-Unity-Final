using UnityEngine;

public class CubeMaterialChanger : MonoBehaviour
{
    public Material originalMaterial; // The original material of the cube
    public Material touchedMaterial; // The material to change to when touched

    private Renderer cubeRenderer;

    private bool isMouseOver = false;

    void Start()
    {
        // Get the Renderer component of the cube
        cubeRenderer = GetComponent<Renderer>();

        // Ensure the original material is set initially
        if (originalMaterial != null)
        {
            cubeRenderer.material = originalMaterial;
        }
    }

    void Update()
{
    CheckMouseHover();
}

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object that collided with this cube is the sphere
        if (collision.gameObject.CompareTag("Sphere"))
        {
            // Change the material to the touched material
            if (touchedMaterial != null)
            {
                cubeRenderer.material = touchedMaterial;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Check if the object exiting collision is the sphere
        if (collision.gameObject.CompareTag("Sphere"))
        {
            // Revert the material back to the original
            if (originalMaterial != null)
            {
                cubeRenderer.material = originalMaterial;
            }
        }
    }
    void CheckMouseHover()
   {
       Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       RaycastHit hit;

       if (Physics.Raycast(ray, out hit))
       {
           if (hit.collider != null && hit.collider.gameObject == gameObject)
           {
               if (!isMouseOver) // Change material only if not already changed
               {
                   isMouseOver = true;
                   cubeRenderer.material = touchedMaterial;
               }
           }
           else
           {
               if (isMouseOver)
               {
                   isMouseOver = false;
                   cubeRenderer.material = originalMaterial;
               }
           }
       }
       else if (isMouseOver)
       {
           isMouseOver = false;
           cubeRenderer.material = originalMaterial;
       }
   }
}
