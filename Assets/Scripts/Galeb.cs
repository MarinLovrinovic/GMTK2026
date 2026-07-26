using System;
using UnityEngine;

public class Galeb : MonoBehaviour, IHittable
{
    [NonSerialized] public Vector2 velocity = new(1, 1);
    public float speed = 1;
    public Vector2 Position => transform.position.xz();

    private bool alive;
    private float fallSpeed = 0f;


    void Start() { alive = true; }

    private void FixedUpdate()
    {
        if (alive) 
        {
            // Face forward
            transform.LookAt(transform.position + velocity.xoy(), Vector3.up);
            // Move
            transform.position += velocity.xoy() * Time.fixedDeltaTime * speed;
        }
        else
        {
            // Fall
            transform.position += Vector3.down * Time.fixedDeltaTime * fallSpeed;
            fallSpeed += 9.81f * Time.fixedDeltaTime;
        }
    }

    public void Hit(float damage)
    {
        Destroy(gameObject);
    }

    public void Freeze(float time)
    {
        return;
    }
}
