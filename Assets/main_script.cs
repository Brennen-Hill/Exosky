using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_script : MonoBehaviour
{
    //Public variables used for initialization
    public ParticleSystem particle_system_prefab;
//    public int star_count = 118218;
    public int planet_count_max = 5765;
    public Camera camera_prefab;
    public TextAsset star_data;
    public TextAsset exoplanet_data;
    public Material clicked_material;

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
    private planet_script clicked_planet;
    private float percent_travelled;
    private float travel_seconds;
    private float travel_power;
    public int distance_multiplier;
    public bool show_planet_UI = true;
    private float lookSpeed;
    private float size_of_earth;
    private float clicked_planet_size;
    private int planet_count_initialized;
    private GameObject[] planets;
    private float planet_UI_size;
    private float angle_to_current_planet;

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
        update_planets();
        update_clicked_planet();
        update_camera(); //This must be called after update_clicked_planet
    }

    private void initialize_particles() {
        int total_particle_count = 0;
        CSVParser.Parse(star_data, 0, 2, (location) => {
            total_particle_count ++;
        });
        particle_system = Instantiate(particle_system_prefab);
        particle_system.Emit(total_particle_count);
        particles = new ParticleSystem.Particle[total_particle_count];
        particle_system.GetParticles(particles);
        int particle_count = 0;
        CSVParser.Parse(star_data, 0, 6, (location) => {
          Vector3 position = new Vector3(location[0] * distance_multiplier, location[1] * distance_multiplier, location[2] * distance_multiplier);
            particles[particle_count].position = position;
            particles[particle_count].startColor = new Color(location[4], location[5], location[6]);
            //create_particle_collider(position, particle_count);
            particle_count++;
        });
        Debug.Log("particle_count");
        Debug.Log(particle_count);
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
            colliderObject.transform.localScale = Vector3.one * 1f; // Set collider size, tweak as needed

            // Add a tag to easily identify the colliders later
            colliderObject.tag = "StarCollider";

            // Store collider reference for later manipulation if needed
            particle_colliders.Add(colliderObject);
        }
    }

    private void initialize_planets() {
        planets = new GameObject[planet_count_max];
        planet_count_initialized = 0;
        CSVParser.Parse(exoplanet_data, 1, 3, (location) => {
            GameObject planet = Instantiate(planet_prefab);
            planet.transform.position = new Vector3(location[1] * distance_multiplier, location[2] * distance_multiplier, location[3] * distance_multiplier);
//            planet.transform.localScale = new Vector3(size_of_earth * distance_multiplier, size_of_earth * distance_multiplier, size_of_earth * distance_multiplier);
            planet.GetComponent<planet_script>().main = this;
            planets[planet_count_initialized] = planet;//new Vector3(location[1], location[2], location[3]);
            planet_count_initialized += 1;
            planet_UI_size = 0.03f;
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
        distance_multiplier = 5; //2000
        size_of_earth = 0.000000002f;
        clicked_planet_size = 10f;

    }
    private void initialize_camera() {
        camera = Instantiate(camera_prefab);
        Vector3 look = new Vector3(0,0,0);
        camera_offset = new Vector3(0, clicked_planet_size * distance_multiplier / 2 + 0.4f, 0);
        camera.transform.position = camera_offset;
        next_location = camera_offset;
        percent_travelled = 1;
        travel_seconds = 2f;
        travel_power = 2f;
        lookSpeed = 1.5f;
    }
    private void update_camera() {
        look = look + new Vector3(-Input.GetAxis("Mouse Y") * lookSpeed, Input.GetAxis("Mouse X") * lookSpeed, 0);
        camera.transform.eulerAngles = look;
        if(percent_travelled != 1) {
            percent_travelled = Mathf.Min(1, percent_travelled + Time.deltaTime / travel_seconds);
            float eased_percent_travelled = Mathf.Pow((1 - Mathf.Cos(percent_travelled * Mathf.PI))/ 2, travel_power);
            camera.transform.position = Vector3.Lerp(last_location, next_location, eased_percent_travelled);
        }
    }

    public void moveCamera(Vector3 new_position, planet_script planet_scr) {
//        camera.transform.position = new_position + camera_offset;
        last_location = camera.transform.position;
        next_location = new_position + camera_offset;
        clicked_planet = planet_scr;
        percent_travelled = 0;
        planet_scr.GetComponent<MeshRenderer>().material = clicked_material;
        //angle_to_current_planet = 

    }

    public void update_planets() {
        for(int i = 0; i < planet_count_initialized; i ++) {
            // Get the distance between the planet and the camera
            float distance = Vector3.Distance(planets[i].transform.position, camera.transform.position);

            // Adjust the scale based on the distance
            float scale;
            if(show_planet_UI) {
                scale = distance * planet_UI_size;
            } else {
                scale = size_of_earth;
            }
            planets[i].transform.localScale = new Vector3(scale, scale, scale);
        }
    }
    public void update_clicked_planet() {
        if(clicked_planet) {
            float distance = Vector3.Distance(clicked_planet.transform.position, camera.transform.position);
            float start_scale = distance * planet_UI_size;
            float end_scale = clicked_planet_size * distance_multiplier;
            float scale = (1 - percent_travelled) * start_scale + (percent_travelled) * end_scale;
            if(scale < start_scale) {
//                scale = start_scale;
            }
//            float scale = Mathf.Max(, );
//            float scale = end_scale;
            clicked_planet.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}