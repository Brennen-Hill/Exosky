using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExoplanetCSVReader
{
    public TextAsset csvFile;
    
    public List<PlanetData> ReadCSV()
    {
        const int maxPlanets = 100; // Set the limit to the number of planets to create
        List<PlanetData> planetDataList = new List<PlanetData>();
        StringReader reader = new StringReader(csvFile.text);

        bool headerSkipped = false;
        int planetCount = 0;

        while (reader.Peek() != -1)
        {
            if (planetCount >= maxPlanets)
            {
                break; // Exit the loop if the maximum number of planets is reached
            }

            var line = reader.ReadLine();
            if (!headerSkipped)
            {
                headerSkipped = true; // Skip the header line
                continue;
            }

            var values = line.Split(',');

            // Only create a PlanetData if there are enough fields available for minimal requirements
            if (values.Length >= 19) // Ensure there are enough columns for all properties
            {
                PlanetData data = ScriptableObject.CreateInstance<PlanetData>();

                data.planetName = values[0];
                data.orbitalPeriod = float.TryParse(values[1], out float orbitalPeriod) ? orbitalPeriod : 365f;
                data.radius = float.TryParse(values[2], out float radius) ? radius : 1f;
                data.mass = float.TryParse(values[3], out float mass) ? mass : 1f;
                data.semiMajorAxis = float.TryParse(values[4], out float semiMajorAxis) ? semiMajorAxis : 1f;
                data.eccentricity = float.TryParse(values[5], out float eccentricity) ? eccentricity : 0f;
                data.insolationFlux = float.TryParse(values[6], out float insolationFlux) ? insolationFlux : 1f;
                data.equilibriumTemperature = float.TryParse(values[7], out float equilibriumTemperature) ? equilibriumTemperature : 1f;
                data.starTemperature = float.TryParse(values[8], out float starTemperature) ? starTemperature : 1f;
                data.starRadius = float.TryParse(values[9], out float starRadius) ? starRadius : 1f;
                data.albedo = float.TryParse(values[10], out float albedo) ? albedo : 0.3f;
                data.density = float.TryParse(values[11], out float density) ? density : 1f;
                data.gravity = float.TryParse(values[12], out float gravity) ? gravity : 1f;
                data.inclination = float.TryParse(values[13], out float inclination) ? inclination : 0f;
                data.orbitalSpeed = float.TryParse(values[14], out float orbitalSpeed) ? orbitalSpeed : 1f;
                data.starLuminosity = float.TryParse(values[15], out float starLuminosity) ? starLuminosity : 1f;
                data.surfacePressure = float.TryParse(values[16], out float surfacePressure) ? surfacePressure : 1f;
                data.surfaceTemperature = float.TryParse(values[17], out float surfaceTemperature) ? surfaceTemperature : 1f;
                data.atmosphereComposition = values[18];

                planetDataList.Add(data);
                planetCount++;
            }
            else
            {
                Debug.LogWarning("Skipping malformed line: " + line);
            }
        }

        return planetDataList;
    }
}