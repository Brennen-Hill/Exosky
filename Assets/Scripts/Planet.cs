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
        
        // Add the PlanetRotation component if not already present
        if (GetComponent<PlanetRotation>() == null)
        {
            gameObject.AddComponent<PlanetRotation>();
        }
    }
}
