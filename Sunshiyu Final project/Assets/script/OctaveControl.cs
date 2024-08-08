using UnityEngine;

public class OctaveControl : MonoBehaviour
{
    public Transform up; // Assign the "up" GameObject in the Inspector
    public Transform down; // Assign the "down" GameObject in the Inspector
    public CsoundNoteTrigger[] noteTriggers; // Assign all the CsoundNoteTrigger components for your cubes
    public int octaveShift = 0; // Tracks the current octave shift

    private const int MIN_OCTAVE_SHIFT = -5; // Corresponding to C0 (0 - 5)
    private const int MAX_OCTAVE_SHIFT = 3; // Corresponding to C7 (7 - 4)

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.transform == up)
                    {
                        ShiftOctave(1);
                    }
                    else if (hit.collider.transform == down)
                    {
                        ShiftOctave(-1);
                    }
                }
            }
        }
    }

    void ShiftOctave(int direction)
    {
        int newOctaveShift = octaveShift + direction;
        if (newOctaveShift >= MIN_OCTAVE_SHIFT && newOctaveShift <= MAX_OCTAVE_SHIFT)
        {
            octaveShift = newOctaveShift;
            AdjustNoteFrequencies();
            Debug.Log("Octave shift: " + octaveShift);
        }
    }

    void AdjustNoteFrequencies()
    {
        foreach (var trigger in noteTriggers)
        {
            trigger.SetOctaveShift(octaveShift);
        }
    }
}
