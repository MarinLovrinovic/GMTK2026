using System;
using UnityEngine;

public class Vehicle : MonoBehaviour, IHittable
{
    public Vector2 velocity = new(1, 1);
    private void FixedUpdate()
    {
        // Move
        transform.position += velocity.xoy() * Time.fixedDeltaTime;
        // Rotate towards movement
        transform.LookAt(transform.position + new Vector3(velocity.x, 0f, velocity.y), Vector3.up);

        // Destroy when out of bounds
        if (Mathf.Abs(transform.position.x) > 10 * Scaler.Scale || Mathf.Abs(transform.position.y) > 6 * Scaler.Scale)
        {
            Destroy(gameObject);    
        }
    }

    public void Hit(float damage)
    {
        Destroy(gameObject);
    }
}
