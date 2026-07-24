using System;
using UnityEngine;

public class Vehicle : MonoBehaviour, IHittable
{
    [NonSerialized] public Vector2 velocity = new(1, 1);
    public float radius = 1;
    public float speed = 1;
    public Vector2 Position => transform.position.xz();
    private void FixedUpdate()
    {
        // Move
        transform.position += velocity.xoy() * Time.fixedDeltaTime;
        // Rotate towards movement
        transform.LookAt(transform.position + velocity.xoy(), Vector3.up);
    }

    public void Hit(float damage)
    {
        Destroy(gameObject);
    }
}
