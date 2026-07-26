using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float lengthInTime;

    private ParticleSystem particleSystem;
    private float radius;

    private void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    protected virtual void doEffects()
    {
        if (particleSystem)
        {
            var shapeModule = particleSystem.shape;
            var emissionModule = particleSystem.emission;
            var mainModule = particleSystem.main;

            shapeModule.radius = radius;
            emissionModule.rateOverTimeMultiplier *= radius * radius;
            mainModule.startLifetimeMultiplier *= radius * radius;
            //mainModule.startSpeedMultiplier *= radius * radius;
        }
    }

    public void sizeAndDestroy(float explosionRadius)
    {
        radius = explosionRadius;
        doEffects();
        Destroy(gameObject, lengthInTime);
    }
}
