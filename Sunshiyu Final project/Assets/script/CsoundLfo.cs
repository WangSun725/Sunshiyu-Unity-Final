using UnityEngine;

public class CsoundLfo : MonoBehaviour
{
    private CsoundUnity csound; // Reference to the CsoundUnity component

    private bool isLFOActive = false; // Tracks the state of the LFO

    void Start()
    {
        // Find the CsoundUnity component in the scene
        csound = FindObjectOfType<CsoundUnity>();
        if (csound == null)
        {
            Debug.LogError("CsoundUnity component not found in the scene.");
        }
    }

    void Update()
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
                Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");

                if (hit.collider.gameObject == gameObject) // Check if the hit object is this GameObject
                {
                    ToggleLFO();
                }
            }
            else
            {
                Debug.Log("Raycast did not hit any object.");
            }
        }
    }

    private void ToggleLFO()
    {
        // Toggle the LFO state
        isLFOActive = !isLFOActive;

        // Send the updated LFO state to Csound
        csound.SetChannel("lfoBypass", isLFOActive ? 0 : 1);

        // Additional debug log to confirm channel setting
        Debug.Log($"Set channel 'lfoBypass' to {(isLFOActive ? 0 : 1)}");

        // Debug message to verify LFO state change
        Debug.Log($"LFO is now {(isLFOActive ? "active" : "bypassed")}");
    }
}
