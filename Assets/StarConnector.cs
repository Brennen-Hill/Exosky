using UnityEngine;
using System.Collections.Generic;

public class StarConnector : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject linePrefab; // Prefab containing the LineRenderer component

    private Vector3 firstStarPosition;
    private bool isFirstStarSelected = false;

    void Start()
    {
        Debug.Log("START");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Debug the direction of the ray
            Debug.DrawRay(ray.origin, ray.direction * 500, Color.red, 2.0f);
            Debug.Log("Ray origin: " + ray.origin + ", Direction: " + ray.direction);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0))
            {

                Debug.Log("Raycast hit: " + hit.collider.name);
                // Check if the clicked object has the tag "StarCollider"
                if (hit.collider != null && hit.collider.CompareTag("StarCollider"))
                {
                    Vector3 starPosition = hit.collider.transform.position;

                    Debug.Log("Star clicked at position: " + starPosition);

                    if (!isFirstStarSelected)
                    {
                        // Store the first star position
                        firstStarPosition = starPosition;
                        isFirstStarSelected = true;
                        Debug.Log("First star selected at position: " + firstStarPosition);
                    }
                    else
                    {
                        // Draw a line between the first and second star
                        CreateLineBetweenPoints(firstStarPosition, starPosition);
                        Debug.Log("Second star selected at position: " + starPosition);
                        Debug.Log("Line drawn between: " + firstStarPosition + " and " + starPosition);
                        isFirstStarSelected = false;
                    }
                }
            }
        }
    }

    void CreateLineBetweenPoints(Vector3 start, Vector3 end)
    {
        // Instantiate a line GameObject from the prefab
        GameObject lineObject = Instantiate(linePrefab);
        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();

        // Set up the LineRenderer properties
        lineRenderer.startWidth = 0.2f;  // Thickness at the start
        lineRenderer.endWidth = 0.2f;   
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
