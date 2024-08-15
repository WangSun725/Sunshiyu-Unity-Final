using UnityEngine;

public class FaderController : MonoBehaviour
{
    public float minPosition = 0.3f; // Minimum Z position for fader control (volume 0)
    public float maxPosition = 2.4f; // Maximum Z position for fader control (volume 1)
    public Axis movementAxis = Axis.Z; // Axis along which the fader moves
    public float movementSpeed = 10f; // Speed of movement

    private bool isDragging = false;
    private Vector3 offset;
    private Plane dragPlane;
    private CsoundUnity csound;

    public enum Axis
    {
        X,
        Y,
        Z
    }

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
                if (hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    dragPlane = new Plane(Vector3.up, transform.position);
                    float distance;
                    dragPlane.Raycast(ray, out distance);
                    offset = transform.position - ray.GetPoint(distance);
                    Debug.Log("Fader clicked: " + gameObject.name);
                }
                else
                {
                    Debug.Log("Clicked on a different object: " + hit.collider.gameObject.name);
                }
            }
            else
            {
                Debug.Log("Nothing was clicked.");
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                Debug.Log("Fader released: " + gameObject.name);
            }
            isDragging = false;
        }

        if (isDragging)
        {
            MoveFader();
        }
    }

    void MoveFader()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (dragPlane.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance) + offset;
            Vector3 newPosition = transform.position;

            switch (movementAxis)
            {
                case Axis.X:
                    newPosition.x = Mathf.Clamp(point.x, minPosition, maxPosition);
                    break;
                case Axis.Y:
                    newPosition.y = Mathf.Clamp(point.y, minPosition, maxPosition);
                    break;
                case Axis.Z:
                    newPosition.z = Mathf.Clamp(point.z, minPosition, maxPosition);
                    break;
            }

            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementSpeed);
            Debug.Log("Fader moving to: " + newPosition);

            // Update the volume slider based on fader position
            UpdateSlider();
        }
    }

    void UpdateSlider()
    {
        if (csound != null)
        {
            // Calculate normalized value based on Z position
            float normalizedValue = (transform.position.z - minPosition) / (maxPosition - minPosition);
            normalizedValue = Mathf.Clamp01(normalizedValue);

            // Send the value to the Csound volume slider channel
            csound.SetChannel("volume", normalizedValue);
            Debug.Log("Slider updated: " + normalizedValue);
        }
    }
}
