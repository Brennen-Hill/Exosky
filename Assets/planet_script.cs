using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planet_script : MonoBehaviour
{
    public main_script main;

    public UIScript main_ui;

    private Material sphereMaterialInstance; // Unique material instance for this object
    private Color originalColor;
    private float originalAlpha;
    private float targetAlpha;
    private float fadeSpeed = 2f; // Speed of the opacity transition

    private Vector3 randomRotationAxis; // Random axis for rotation
    public float rotationSpeed = 0.00000001f; // Rotation speed around the axis

    public string planet_name = "";

    void Start()
    {
        // Create a unique instance of the material for this object
        sphereMaterialInstance = GetComponent<Renderer>().material;

        // Store the original material color and transparency
        originalColor = sphereMaterialInstance.color;
        originalAlpha = originalColor.a;

        // Set the initial target alpha to the original alpha value
        targetAlpha = originalAlpha;

        randomRotationAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    void Update()
    {
        // Gradually change the alpha value toward the targetAlpha
        Color currentColor = sphereMaterialInstance.color;
        currentColor.a = Mathf.Lerp(currentColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
        sphereMaterialInstance.color = currentColor;

        transform.Rotate(randomRotationAxis, rotationSpeed * Time.deltaTime);
    }

    void OnMouseEnter()
    {
        targetAlpha = originalAlpha * 5f;
        main_ui.setHoverPlanetName(planet_name);
    }

    void OnMouseExit()
    {
        // When the mouse exits, reset the target alpha to the original alpha value
        targetAlpha = originalAlpha;
        main_ui.clearHoverPlanetName();
    }


/*
    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
    }
*/
    void OnMouseDown() {
        //OnMouseDown is called when the user has pressed the mouse button while over the Collider.
        if (!main.constellationMode) {
            Debug.Log("Mouse is down on the element");
            main.moveCamera(transform.position, this);
            main_ui.setPlanetName(planet_name);
        }
    }
}
