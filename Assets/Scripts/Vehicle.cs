using System;
using System.Collections;
using UnityEngine;

public class Vehicle : MonoBehaviour, IHittable
{
    [NonSerialized] public Vector2 velocity = new(1, 1);
    public float radius = 1;
    public float speed = 1;
    public float turningSpeed = 5; // degrees per second
    public float turningDirection = 0; // -1 is left, 1 is right, 0 is no turn
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
            // Turn
            velocity = velocity.RotateVector2ByDegrees(turningDirection * turningSpeed * Time.deltaTime);
            transform.LookAt(transform.position + velocity.xoy(), Vector3.up);
            
            // Move
            transform.position += velocity.xoy() * Time.fixedDeltaTime;
        }
    }

    private IEnumerator waitFrozen(float time)
    {
        if (FreezeBomb.freezeShip) { transform.Find("Frozen").Find("IceShip").gameObject.SetActive(true); }
        else { transform.Find("Frozen").Find("IceCube").gameObject.SetActive(true); }
        isFrozen = true;
        Vector2 previousVelocity = velocity;
        velocity = Vector2.zero;

        yield return new WaitForSecondsRealtime(time);

        velocity = previousVelocity;
        isFrozen = false;
        if (FreezeBomb.freezeShip) { transform.Find("Frozen").Find("IceShip").gameObject.SetActive(false); }
        else { transform.Find("Frozen").Find("IceCube").gameObject.SetActive(false); }
    }

    public void Hit(float damage)
    {
        Destroy(gameObject);
    }

    public void Freeze(float time)
    {
        if (isFrozen) return;
        StartCoroutine(waitFrozen(time));
    }
}
