using UnityEngine;
using System.Collections.Generic; // Add this line to use Dictionary

public class CsoundNoteTrigger : MonoBehaviour
{
    public CsoundUnity csound; // Reference to the CsoundUnity component
    public int noteIndex; // Index of the note in the scale (0 = tonic, 1 = supertonic, etc.)
    private string currentScale = "C"; // Tracks the current scale, initialized to "C"

    // Base frequencies for root notes of C3 to B3 for each major scale
    private readonly Dictionary<string, float> scaleRootFrequencies = new Dictionary<string, float>
    {
        { "C", 130.81f }, // C3
        { "D", 146.83f }, // D3
        { "E", 164.81f }, // E3
        { "F", 174.61f }, // F3
        { "G", 196.00f }, // G3
        { "A", 220.00f }, // A3
        { "B", 246.94f }  // B3
    };

    // Define major scale intervals: whole step = 2, half step = 1
    private int[] majorScaleIntervals = { 2, 2, 1, 2, 2, 2, 1 };

    private float currentFrequency;
    private int currentOctaveShift = 0; // Tracks the current octave shift

    void Start()
    {
        if (csound == null)
        {
            csound = FindObjectOfType<CsoundUnity>();
            if (csound == null)
            {
                Debug.LogError("CsoundUnity component not found in the scene.");
            }
        }

        UpdateFrequency(currentOctaveShift, currentScale); // Initialize frequency with C major and no octave shift
    }

    void Update()
    {
        CheckMouseClick();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sphere"))
        {
            PlayNote();
        }
    }

    void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    PlayNote();
                }
            }
        }
    }

    void PlayNote()
    {
        if (csound != null)
        {
            // Send score event to Csound to play the note
            csound.SendScoreEvent($"i \"Pluck\" 0 0.5 {currentFrequency} 0.5");
            Debug.Log($"Playing note with frequency: {currentFrequency} Hz");
        }
        else
        {
            Debug.LogError("CsoundUnity component is null. Cannot send score event.");
        }
    }

    public void SetOctaveShift(int octaveShift)
    {
        if (octaveShift >= -3 && octaveShift <= 3) // Range to ensure octave shift is within C0 to C7
        {
            currentOctaveShift = octaveShift;
            UpdateFrequency(octaveShift, currentScale);
        }
    }

    public void SetScale(string scale)
    {
        currentScale = scale; // Update current scale
        UpdateFrequency(currentOctaveShift, currentScale);
    }

    private void UpdateFrequency(int octaveShift, string scale)
    {
        if (!scaleRootFrequencies.TryGetValue(scale, out float rootFrequency))
        {
            Debug.LogError($"Scale {scale} not found in root frequencies. Defaulting to C.");
            rootFrequency = scaleRootFrequencies["C"];
        }

        // Calculate the cumulative interval from the root for the note index
        int intervalSum = 0;
        for (int i = 0; i < noteIndex; i++)
        {
            intervalSum += majorScaleIntervals[i % majorScaleIntervals.Length];
        }

        // Calculate the frequency of the current note based on the intervals
        currentFrequency = rootFrequency * Mathf.Pow(2, intervalSum / 12.0f) * Mathf.Pow(2, octaveShift);

        Debug.Log($"Updated frequency for note index {noteIndex} in {scale} major: {currentFrequency} Hz");
    }
}
