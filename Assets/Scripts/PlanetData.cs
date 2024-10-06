using UnityEngine;

[CreateAssetMenu(fileName = "NewPlanetData", menuName = "Planet/Planet Data")]
public class PlanetData : ScriptableObject
{
    public string planetName; // Identifier of the exoplanet (pl_name)
    public float orbitalPeriod; // The time it takes for the planet to orbit its host star (pl_orbper)
    public float radius; // Radius of the exoplanet in Earth radii (pl_rade)
    public float mass; // Mass of the exoplanet in Earth masses (pl_bmasse)
    public float semiMajorAxis; // The average distance between the exoplanet and its host star (pl_orbsmax)
    public float eccentricity; // Describes the shape of the orbit (how elliptical it is) (pl_orbeccen)
    public float insolationFlux; // The amount of energy received from the host star (pl_insol)
    public float equilibriumTemperature; // The temperature of the planet assuming a simplified atmosphere (pl_eqt)
    public float starTemperature; // Temperature of the host star (st_teff)
    public float starRadius; // Radius of the host star (st_rad)
    public float albedo; // Reflectivity of the exoplanet's surface (pl_albedo)
    public float density; // Density of the exoplanet in g/cm³ (pl_dens)
    public float gravity; // Surface gravity of the exoplanet in cm/s² (pl_grav)
    public float inclination; // Inclination of the exoplanet's orbit in degrees (pl_orbincl)
    public float orbitalSpeed; // Average orbital speed of the exoplanet in km/s (pl_orbspeed)
    public float starLuminosity; // Luminosity of the host star (st_lum)
    public float surfacePressure; // Surface pressure of the exoplanet (pl_surfpressure)
    public float surfaceTemperature; // Surface temperature of the exoplanet (pl_surf_temp)
    public string atmosphereComposition; // Composition of the exoplanet's atmosphere (pl_atmcomp)
}
