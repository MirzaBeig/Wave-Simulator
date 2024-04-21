using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSimulatorParticleSystem : MonoBehaviour
{
    public WaveSimulator waveSimulator;

    public float minSpeed = 0.1f;
    public float inputRadius = 10.0f;

    [Space]

    public float collisionForce = 0.1f;

    ParticleSystem particleSystem;
    List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void Update()
    {

    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = particleSystem.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            // Raycast from the collision point (backwards a tiny bit [0.1f]) to the collider's surface.

            ParticleCollisionEvent collisionEvent = collisionEvents[i];

            float collisionSpeed = collisionEvent.velocity.magnitude;

            // Skip if the particle is too slow.
            // Basically, I want to simulate "OnParticleCollisionEnter".

            if (collisionSpeed < minSpeed)
            {
                continue;
            }

            Vector3 contactPoint = collisionEvent.intersection;
            Vector3 contactNormal = collisionEvent.normal;

            Ray ray = new()
            {
                origin = contactPoint + (contactNormal * 0.1f),
                direction = -contactNormal
            };

            if ((collisionEvent.colliderComponent as Collider).Raycast(ray, out RaycastHit hit, 0.2f))
            {
                Vector2 collisionPixelCoord = hit.textureCoord * waveSimulator.size;
                waveSimulator.AddInput(collisionPixelCoord, inputRadius * particleSystem.main.startSize.constant * (collisionSpeed * collisionForce));
            }
        }
    }
}
