using UnityEngine;

public class Planet : MonoBehaviour
{
    public PlanetData planetData;

    void Start()
    {
        if (planetData != null)
        {
            ApplyData(planetData);
        }
    }

    public void ApplyData(PlanetData data)
    {
        planetData = data;

        // Set properties on the GameObject based on data
        transform.localScale = Vector3.one * data.radius;
        GetComponent<MeshRenderer>().material.color = data.surfaceColor;
        // Apply any other properties, like mass or rotation, if necessary
    }
}
