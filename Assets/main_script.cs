using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_script : MonoBehaviour
{
    public ParticleSystem particle_system_prefab;
    public Camera camera_prefab;
    private Camera camera;
    public GameObject planet_prefab;
    private ParticleSystem particle_system;
    private ParticleSystem.Particle[] particles;

    private Vector3 look;
    private Vector3 camera_offset;
    private Vector3 last_location;
    private Vector3 next_location;
    private float percent_travelled;
    private float travel_seconds;

    // Start is called before the first frame update
    void Start()
    {
        camera = Instantiate(camera_prefab);
        Vector3 look = new Vector3(0,0,0);
        camera_offset = new Vector3(0, 5.4f, 0);
        camera.transform.position = camera_offset;
        next_location = camera_offset;
        percent_travelled = 1;
        travel_seconds = 10f;

        particle_system = Instantiate(particle_system_prefab);
        Vector3[] locations = get_locations(100000, -1000, 1000); //1000000, -1000, 1000
        int num_particles = locations.Length;
        particle_system.Emit(num_particles);
        particles = new ParticleSystem.Particle[num_particles];
        int num_particles2 = particle_system.GetParticles(particles);
        for(int i = 0; i < num_particles; i ++) {
            particles[i].position = locations[i];
        }
        particle_system.SetParticles(particles);

        Vector3[] planet_locations = get_locations(100, -1000, 1000); 
        for(int i = 0; i < planet_locations.Length; i ++) {
            GameObject planet = Instantiate(planet_prefab);
            planet.transform.position = planet_locations[i];
            planet.GetComponent<planet_script>().main = this;
        }
        GameObject my_planet = Instantiate(planet_prefab);
        my_planet.transform.position = new Vector3(0,0,0);
        my_planet.GetComponent<planet_script>().main = this;
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
        /*
        float lookSpeed = 1.5f;
        float y = Input.GetAxis("Mouse X");
        float x = Input.GetAxis("Mouse Y");
        Debug.Log(x + ":" + y);
        Vector3 rotateValue = new Vector3(x * lookSpeed, y * -1 * lookSpeed, 0);
        camera.transform.eulerAngles = transform.eulerAngles - rotateValue;
        */

        float lookSpeed = 1.5f;

        look = look + new Vector3(-Input.GetAxis("Mouse Y") * lookSpeed, Input.GetAxis("Mouse X") * lookSpeed, 0);
//        look.x = Mathf.Clamp(look.x, -80, 80);
        camera.transform.eulerAngles = look;
        //camera.transform.position = position;

        if(percent_travelled != 1) {
            percent_travelled = Mathf.Min(1, percent_travelled + Time.deltaTime / travel_seconds);
            float power = 2;
            float eased_percent_travelled = Mathf.Pow((1 - Mathf.Cos(percent_travelled * Mathf.PI))/ 2, power);
//            camera.transform.position = next_location * percent_travelled + last_location * (1 - percent_travelled);
            camera.transform.position = Vector3.Lerp(last_location, next_location, eased_percent_travelled);
        }
    }

    public void moveCamera(Vector3 new_position) {
//        camera.transform.position = new_position + camera_offset;
        last_location = camera.transform.position;
        next_location = new_position + camera_offset;
        percent_travelled = 0;
    }
}