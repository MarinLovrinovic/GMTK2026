using System;
using System.Collections;
using UnityEngine;

public class Vehicle : MonoBehaviour, IHittable
{
    [NonSerialized] public Vector2 velocity = new(1, 1);
    public float radius = 1;
    public float speed = 1;
    public float turningSpeed = 5; // degrees per second
    public int turningDirection = 0; // -1 is left, 1 is right, 0 is no turn
    public Vector2 Position => transform.position.xz();

    private bool isFrozen;

    private void Start()
    {
        isFrozen = false;
    }

    private void FixedUpdate()
    {
        if (!isFrozen)
        {
            // Move
            transform.position += velocity.xoy() * Time.fixedDeltaTime;
            // Rotate towards movement
            transform.LookAt(transform.position + velocity.xoy(), Vector3.up);
        }
    }

    private IEnumerator waitFrozen(float time)
    {
        isFrozen = true;
        yield return new WaitForSecondsRealtime(time);
        isFrozen = false;
    }

    public void Hit(float damage)
    {
        Destroy(gameObject);
    }

    public void Freeze(float time)
    {
        StartCoroutine(waitFrozen(time));
    }
}
