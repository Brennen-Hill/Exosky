using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExoplanetCSVReader
{
    public TextAsset csvFile; // Assign your CSV file in the Inspector

    public List<PlanetData> ReadCSV()
    {
        const int maxColumnCount = 5; // Set the limit to the number of columns to process
        const int maxPlanets = 100; // Set the limit to the number of planets to create
        List<PlanetData> planetDataList = new List<PlanetData>();
        StringReader reader = new StringReader(csvFile.text);

        bool headerSkipped = false;
        int planetCount = 0; // Remember to instantiated planets

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
            int columnCount = Mathf.Min(values.Length, maxColumnCount); // Ensure we don't exceed maxColumnCount or the number of available values

            // Only create a PlanetData if there are enough fields available for minimal requirements
            if (columnCount >= 3) // Assuming you need at least 3 columns for minimal planet data
            {
                PlanetData data = ScriptableObject.CreateInstance<PlanetData>();

                // Dynamically process columns up to the limit
                for (int i = 0; i < columnCount; i++)
                {
                    switch (i)
                    {
                        case 0:
                            data.planetName = values[i];
                            break;
                        case 1:
                            data.radius = float.TryParse(values[i], out float radius) ? radius : 1f; // Default to 1 if parsing fails
                            break;
                        case 2:
                            data.mass = float.TryParse(values[i], out float mass) ? mass : 1f; // Default to 1 if parsing fails
                            break;
                        // Add more cases for additional columns if needed, up to maxColumnCount
                        case 3:
                            // Example: assign some other property if a fourth column exists
                            data.surfaceColor = new Color(Random.value, Random.value, Random.value); // Assign a random color for demonstration
                            break;
                        case 4:
                            // Example: assign some additional property if a fifth column exists
                            data.orbitalPeriod = float.TryParse(values[i], out float orbitalPeriod) ? orbitalPeriod : 365f; // Example property
                            break;
                    }
                }

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