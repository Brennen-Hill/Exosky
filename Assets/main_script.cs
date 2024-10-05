using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_script : MonoBehaviour
{
    public ParticleSystem particle_system_prefab;
    private ParticleSystem particle_system;
    private ParticleSystem.Particle[] particles;

    // Start is called before the first frame update
    void Start()
    {
        particle_system = Instantiate(particle_system_prefab);
        Vector3[] locations = {new Vector3(0, 0, 1), new Vector3(1, 0, 0)};
        int num_particles = locations.Length;
        particle_system.Emit(num_particles);

        particles = new ParticleSystem.Particle[num_particles];

        int num_particles2 = particle_system.GetParticles(particles);
        Debug.Log(num_particles);
        Debug.Log(num_particles2);
        for(int i = 0; i < num_particles; i ++) {
            particles[i].position = locations[i];
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
