using UnityEngine;
using System.Collections.Generic;

public class PlanetManager : MonoBehaviour
{
    public Planet planetPrefab; // Reference to the Planet prefab
    public TextAsset exoplanetCSV; // CSV containing exoplanet data
    public Transform planetsParent; // Reference to the "Planets" GameObject


    void Start()
    {
        // Create an instance of the CSV reader
        ExoplanetCSVReader csvReader = new ExoplanetCSVReader();
        csvReader.csvFile = exoplanetCSV;

        // Read data from the CSV and create planets
        List<PlanetData> planetDataList = csvReader.ReadCSV();
        // foreach (var planetData in planetDataList)
        // {
        //     CreatePlanet(planetData);
        // }
        CreatePlanet(planetDataList[0]);
    }

    public void CreatePlanet(PlanetData template)
    {
        Planet newPlanet = Instantiate(planetPrefab);
        newPlanet.ApplyData(template);

        // Set a random or computed position for the planets (e.g., orbiting a star)
        // newPlanet.transform.position = Random.onUnitSphere * Random.Range(5f, 20f);

        // Set the new planet as a child of the "Planets" GameObject
        newPlanet.transform.SetParent(planetsParent);
        // Set the layer of the planet to the "Planets" layer
        newPlanet.gameObject.layer = LayerMask.NameToLayer("PlanetLayer");
    }
}
