using UnityEngine;

public class WaveformButtonSelector : MonoBehaviour
{
    public CsoundUnity csound; // Reference to the CsoundUnity component

    // Enum to define different waveforms
    public enum Waveform
    {
        Sine = 1,
        Saw = 2,
        Square = 3,
        Triangle = 4
    }

    // Assign the waveform for this button in the Inspector
    public Waveform assignedWaveform;

    // Reference to the stand child object
    public GameObject buttonObject;

    private void Start()
    {
        if (csound == null)
        {
            csound = FindObjectOfType<CsoundUnity>();
            if (csound == null)
            {
                Debug.LogError("CsoundUnity component not found in the scene.");
                return;
            }
        }

        if (buttonObject == null)
        {
            Debug.LogError("Stand object not assigned. Please assign the stand child object in the Inspector.");
        }

        // Ensure the collider is attached and enabled
        BoxCollider collider = buttonObject.GetComponent<BoxCollider>();
        if (collider == null)
        {
            Debug.LogError("No BoxCollider found on the button object. Please add a BoxCollider.");
        }
        else if (!collider.enabled)
        {
            Debug.LogError("BoxCollider is disabled on the button object. Please enable it.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check the raycast length and mask as needed
            float maxRaycastDistance = 100f; // Set an appropriate max distance
            LayerMask layerMask = LayerMask.GetMask("Default"); // Adjust this to your specific layer setup

            if (Physics.Raycast(ray, out hit, maxRaycastDistance, layerMask))
            {
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
                // Check if the raycast hit the stand object
                if (hit.collider.gameObject == buttonObject)
                {
                    Debug.Log($"Clicked stand of: {gameObject.name}, assigned waveform: {assignedWaveform}");
                    SetWaveform((int)assignedWaveform);
                }
            }
        }
    }

    private void SetWaveform(int table)
    {
        csound.SetChannel("waveform", table);
        Debug.Log($"Waveform set to {assignedWaveform} (Table: {table})");
    }
}
