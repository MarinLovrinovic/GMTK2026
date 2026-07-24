using System.Collections;
using UnityEngine;

public class Vehicle : MonoBehaviour, IHittable
{
    public Vector2 velocity = new(1, 1);

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
            transform.LookAt(transform.position + new Vector3(velocity.x, 0f, velocity.y), Vector3.up);
        }

        // Destroy when out of bounds
        if (Mathf.Abs(transform.position.x) > 10 * Scaler.Scale || Mathf.Abs(transform.position.y) > 6 * Scaler.Scale)
        {
            Destroy(gameObject);    
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
