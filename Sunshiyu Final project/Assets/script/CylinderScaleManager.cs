using UnityEngine;

public class CylinderScaleManager : MonoBehaviour
{
    public CsoundNoteTrigger[] noteTriggers; // Assign all the CsoundNoteTrigger components for your cubes
    public string scaleName; // Assign the scale this cylinder should trigger

    private static CylinderScaleManager currentlyActiveCylinder; // Tracks the currently active cylinder

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
                if (hit.collider.gameObject == gameObject) // Check if the hit object is this cylinder
                {
                    ChangeScale();
                }
            }
        }
    }

    private void ChangeScale()
    {
        if (currentlyActiveCylinder != null)
        {
            // If the same cylinder is clicked again, deselect it
            if (currentlyActiveCylinder == this)
            {
                currentlyActiveCylinder = null;
                return;
            }
        }

        // Activate this cylinder and change scale
        currentlyActiveCylinder = this;
        SetScale();
    }

    private void SetScale()
    {
        string newScale = scaleName; // Use the scale assigned to this cylinder
        foreach (var trigger in noteTriggers)
        {
            trigger.SetScale(newScale);
        }
        Debug.Log($"Scale changed to: {newScale} major");
    }
}
