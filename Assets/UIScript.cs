using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIScript : MonoBehaviour
{

    Label planetNameLabel;
    private Button toggleButton;
    private VisualElement controlContainer;
    private bool controlsVisible = true;
    private bool constellationMode = false;
    private VisualElement rootVisualElement;

    string planetName = "Choose an Exoplanet";
    // Start is called before the first frame update
    void OnEnable()
    {
        // Get the root visual element from the UI Document
        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

        // Find the Label by name
        planetNameLabel = rootVisualElement.Q<Label>("PlanetName");
        setPlanetName("Earth");

        toggleButton = rootVisualElement.Q<Button>("control-toggle");
        controlContainer = rootVisualElement.Q<VisualElement>("control-container");

        // Set initial button text
        toggleButton.text = "Close Controls";

        // Register the button click event
        toggleButton.clicked += ToggleControlVisibility;
    }


    private void ToggleControlVisibility()
    {
        controlsVisible = !controlsVisible;

        if (controlsVisible)
        {
            // Show labels and change button text
            toggleButton.text = "Close Controls";
            foreach (var child in controlContainer.Children())
            {
                if (child is Label)
                {
                    child.style.display = DisplayStyle.Flex;
                }
            }
        }
        else
        {
            // Hide labels and change button text
            toggleButton.text = "Open Controls";
            foreach (var child in controlContainer.Children())
            {
                if (child is Label)
                {
                    child.style.display = DisplayStyle.None;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            constellationMode = !constellationMode;
            if (constellationMode) {
                rootVisualElement.style.display = DisplayStyle.None;
            } else {
                rootVisualElement.style.display = DisplayStyle.Flex;
            }
        }
    }

    public void setHoverPlanetName(string name) {
        planetNameLabel.text = "Hovering: " + name;
    }

    public void clearHoverPlanetName() {
        planetNameLabel.text = planetName;
    }

    public void setPlanetName(string name) {
        planetName = "On: " + name;
        planetNameLabel.text = planetName;
    }
}
