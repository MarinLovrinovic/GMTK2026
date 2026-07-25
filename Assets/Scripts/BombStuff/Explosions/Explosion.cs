using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [NonSerialized] public float explosionRadius;
    void Start()
    {
        doEffects();
        Destroy(gameObject, 1);
    }

    protected virtual void doEffects()
    {
        // if (particleSystem)
        // {
        //     ParticleSystem.ShapeModule shape = particleSystem.shape;
        //     particleSystem.shape = shape;
        //
        // }
    }
}
