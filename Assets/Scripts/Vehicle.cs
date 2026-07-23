using System;
using UnityEngine;

public class Vehicle : MonoBehaviour, IHittable
{
    public Vector2 velocity = new(1, 1);
    private void FixedUpdate()
    {
        transform.position += velocity.xyo() * Time.fixedDeltaTime;
        transform.rotation = velocity.Vector2ToQuaternion();
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
