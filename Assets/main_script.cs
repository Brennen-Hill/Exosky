using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_script : MonoBehaviour
{
    //Public variables used for initialization
    public ParticleSystem particle_system_prefab;
    public int star_count = 100000;
    public int planet_count = 5765;
    public Camera camera_prefab;
    public TextAsset star_data;
    public TextAsset exoplanet_data;

    //private variables used internally
    private Camera camera;
    public GameObject planet_prefab;
    public GameObject collider_prefab;
    private ParticleSystem particle_system;
    private ParticleSystem.Particle[] particles;
    private List<GameObject> particle_colliders = new List<GameObject>();
    private Vector3 look;
    private Vector3 camera_offset;
    private Vector3 last_location;
    private Vector3 next_location;
    private float percent_travelled;
    private float travel_seconds;
    private float look_speed;
    private float travel_power;
    private int distance_multiplier;
    private float lookSpeed;
    private float size_of_earth;
    private Vector3[] planets;

    // Start is called before the first frame update
    void Start()
    {
        initialize_globals();
        initialize_camera();

        initialize_particles();
        initialize_planets();
    }

    private Vector3[] get_locations(int num_locations, int min_val, int max_val) {
        //{new Vector3(0, 0, 1), new Vector3(1, 0, 0)};
        Vector3[] new_locations = new Vector3[num_locations];
        for(int i = 0; i < num_locations; i ++) {
            new_locations[i] = new Vector3(Random.Range(min_val, max_val), Random.Range(min_val, max_val), Random.Range(min_val, max_val));
        }
        return new_locations;
    }

    // Update is called once per frame
    void Update() {
        update_camera();
    }

    private void initialize_particles() {
        particle_system = Instantiate(particle_system_prefab);
        particle_system.Emit(star_count);
        particles = new ParticleSystem.Particle[star_count];
        particle_system.GetParticles(particles);
        int particle_count = 0;
        CSVParser.Parse(star_data, 0, 2, (location) => {
          Vector3 position = new Vector3(location[0] * distance_multiplier, location[1] * distance_multiplier, location[2] * distance_multiplier);
            particles[particle_count].position = position;
            //create_particle_collider(position, particle_count);
            particle_count++;
        });
        particle_system.SetParticles(particles);

        create_colliders_for_particles();
        
    }

        // Create a collider for each particle
    private void create_colliders_for_particles()
    {
        // Loop through particles and create colliders at their positions
        for (int i = 0; i < particles.Length; i++)
        {
            Vector3 position = particles[i].position;

            // Instantiate a collider GameObject at the particle's position
            GameObject colliderObject = Instantiate(collider_prefab, position, Quaternion.identity);
            colliderObject.transform.localScale = Vector3.one * 5f; // Set collider size, tweak as needed

            // Add a tag to easily identify the colliders later
            colliderObject.tag = "StarCollider";

            // Store collider reference for later manipulation if needed
            particle_colliders.Add(colliderObject);
        }
    }

    private void initialize_planets() {
        planets = new Vector3[planet_count];
        int sphere_count = 0;
        CSVParser.Parse(exoplanet_data, 1, 3, (location) => {
            GameObject planet = Instantiate(planet_prefab);
            planet.transform.position = new Vector3(location[1] * distance_multiplier, location[2] * distance_multiplier, location[3] * distance_multiplier);
            planet.transform.localScale = new Vector3(size_of_earth * distance_multiplier, size_of_earth * distance_multiplier, size_of_earth * distance_multiplier);
            planet.GetComponent<planet_script>().main = this;
            planets[sphere_count] = new Vector3(location[1], location[2], location[3]);
            sphere_count += 1;
        });


//        for(int i = 0; i < planet_locations.Length; i ++) {
//            GameObject planet = Instantiate(planet_prefab);
//            planet.transform.position = planet_locations[i];
//            planet.GetComponent<planet_script>().main = this;
//        }

//        GameObject my_planet = Instantiate(planet_prefab);
//        my_planet.transform.position = new Vector3(0,0,0);
//        my_planet.GetComponent<planet_script>().main = this;
    }
    private void initialize_globals() {
        distance_multiplier = 10;
        size_of_earth = 0.1f; //0.000000002f;
    }
    private void initialize_camera() {

        camera = Instantiate(camera_prefab);
        Vector3 look = new Vector3(0,0,0);
        camera_offset = new Vector3(0, size_of_earth * distance_multiplier + 0.4f, 0);
        camera.transform.position = camera_offset;
        next_location = camera_offset;
        percent_travelled = 1;
        travel_seconds = 1.5f;
        look_speed = 1.5f;
        travel_power = 2f;
        lookSpeed = 1.5f;
    }
    private void update_camera() {
        look = look + new Vector3(-Input.GetAxis("Mouse Y") * lookSpeed, Input.GetAxis("Mouse X") * lookSpeed, 0);
        camera.transform.eulerAngles = look;
        if(percent_travelled != 1) {
            percent_travelled = Mathf.Min(1, percent_travelled + Time.deltaTime / travel_seconds);
            float power = 2;
            float eased_percent_travelled = Mathf.Pow((1 - Mathf.Cos(percent_travelled * Mathf.PI))/ 2, power);
            camera.transform.position = Vector3.Lerp(last_location, next_location, eased_percent_travelled);
        }
    }

    public void moveCamera(Vector3 new_position) {
//        camera.transform.position = new_position + camera_offset;
        last_location = camera.transform.position;
        next_location = new_position + camera_offset;
        percent_travelled = 0;
    }

    public void show_planets() {

    }
}