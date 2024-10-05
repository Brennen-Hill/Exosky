using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ExoplanetCSVReader : MonoBehaviour
{
    public TextAsset csvFile; // Assign your CSV file in the Inspector

    public List<PlanetData> ReadCSV()
    {
        List<PlanetData> planetDataList = new List<PlanetData>();
        StringReader reader = new StringReader(csvFile.text);

        bool headerSkipped = false;
        while (reader.Peek() != -1)
        {
            var line = reader.ReadLine();
            if (!headerSkipped)
            {
                headerSkipped = true; // Skip the header line
                continue;
            }
            
            var values = line.Split(',');

            PlanetData data = ScriptableObject.CreateInstance<PlanetData>();
            data.planetName = values[0];
            data.radius = float.Parse(values[1]);
            data.mass = float.Parse(values[2]);
            data.surfaceColor = new Color(Random.value, Random.value, Random.value);
            // Continue parsing as needed

            planetDataList.Add(data);
        }
        return planetDataList;
    }
}
