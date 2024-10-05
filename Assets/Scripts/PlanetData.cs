using UnityEngine;

[CreateAssetMenu(fileName = "NewPlanetData", menuName = "Planet/Planet Data")]
public class PlanetData : ScriptableObject
{
    public string planetName;
    public float radius;
    public float mass;
    public Color surfaceColor;
    public float orbitalPeriod;
    public float eccentricity;
    public float temperature;
    // Add more properties as needed
}
