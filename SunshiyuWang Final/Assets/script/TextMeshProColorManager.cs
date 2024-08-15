using UnityEngine;
using TMPro;

public class TextMeshProColorManager : MonoBehaviour
{
    // Array to hold the GameObjects containing the TextMeshPro components
    public GameObject[] textObjects;
    // Array to hold the GameObjects acting as buttons
    public GameObject[] buttonObjects;

    public Color defaultColor = Color.white; // The default color for the text
    public Color selectedColor = Color.yellow; // The color when a button is selected

    private int selectedIndex = -1; // Index of the currently selected text (-1 means none)

    void Start()
    {
        // Ensure each button has a collider for raycasting
        foreach (var button in buttonObjects)
        {
            if (button.GetComponent<Collider>() == null)
            {
                Debug.LogError($"GameObject {button.name} does not have a Collider attached!");
            }
        }

        // Set all text colors to the default at the start
        UpdateTextColors();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Perform a raycast from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object is one of our button objects
                for (int i = 0; i < buttonObjects.Length; i++)
                {
                    if (hit.collider.gameObject == buttonObjects[i])
                    {
                        Debug.Log($"Clicked button: {buttonObjects[i].name}");
                        OnButtonClicked(i);
                        break;
                    }
                }
            }
        }
    }

    void OnButtonClicked(int index)
    {
        // Update the selected index
        if (selectedIndex == index)
        {
            // If the same button is clicked again, deselect it
            selectedIndex = -1;
        }
        else
        {
            // Otherwise, select the new index
            selectedIndex = index;
        }

        // Update the colors based on the selected button
        UpdateTextColors();
    }

    void UpdateTextColors()
    {
        for (int i = 0; i < textObjects.Length; i++)
        {
            TextMeshPro textMesh = textObjects[i].GetComponent<TextMeshPro>();
            if (textMesh != null)
            {
                if (selectedIndex == i)
                {
                    // Change the color of the selected text
                    textMesh.color = selectedColor;
                }
                else
                {
                    // Reset the color of all other texts to default
                    textMesh.color = defaultColor;
                }
            }
            else
            {
                Debug.LogWarning($"TextMeshPro component not found on {textObjects[i].name}");
            }
        }
    }
}
