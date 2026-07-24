using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private Vehicle vehicle;
    private float timeUntilNextVehicle = 1;
    private List<Vehicle> vehicles = new();
    
    private void Update()
    {
        vehicles.RemoveAll(v =>
        {
            if (!v) return true;
            
            Vector2 pos = v.Position;
            if (Mathf.Abs(pos.x) > 10 * Scaler.Scale || Mathf.Abs(pos.y) > 6 * Scaler.Scale) // vehicle out of bounds
            {
                Destroy(gameObject);
                return true;
            }
            return false;
        });
        
        timeUntilNextVehicle -= Time.deltaTime;
        if (timeUntilNextVehicle <= 0)
        {
            SpawnVehicle();
            timeUntilNextVehicle = Random.Range(0.2f, 2f);
        }
    }

    private void SpawnVehicle()
    {
        Vector2 newVehiclePosition = GetInitialVehicleLocation();
            
        Vehicle newVehicle = Instantiate(vehicle, GetInitialVehicleLocation().xoy(), Quaternion.identity);

        Vector2 velocity = -newVehicle.Position.normalized.RandomVectorDeviation(40) * newVehicle.speed;
        float angleIncrement = 2;
        for (int i = 0; i < 50; i++) // try to find a direction that will not collide with any other ships 
        {
            bool anyCollisions = false;
            foreach (Vehicle otherVehicle in vehicles)
            {
                anyCollisions |= MathHelper.MovingCirclesIntersect(
                    newVehiclePosition, velocity, newVehicle.radius,
                    otherVehicle.Position, otherVehicle.velocity, otherVehicle.radius);
            }
            if (!anyCollisions)
                break;
            velocity.RotateVector2ByDegrees(angleIncrement);
            angleIncrement = -(angleIncrement + Mathf.Sign(angleIncrement) * 2);
            if (i == 24)
                Debug.Log("failed to find path");
        }
        newVehicle.velocity = velocity;

        vehicles.Add(newVehicle);
    }

    private Vector2 GetInitialVehicleLocation()
    {
        float edgeDistance = Random.Range(0f, (2f * 18f + 2f * 11f) * Scaler.Scale);
        Vector2 position = new Vector2(-9f, 5.5f) * Scaler.Scale;

        position += Vector2.right * Mathf.Clamp(edgeDistance, 0, 18f * Scaler.Scale);
        edgeDistance -= 18f * Scaler.Scale;
        if (edgeDistance <= 0f) return position;

        position += Vector2.down * Mathf.Clamp(edgeDistance, 0, 11f * Scaler.Scale);
        edgeDistance -= 11f * Scaler.Scale;
        if (edgeDistance <= 0f) return position;

        position += Vector2.left * Mathf.Clamp(edgeDistance, 0, 18f * Scaler.Scale);
        edgeDistance -= 18f * Scaler.Scale;
        if (edgeDistance <= 0f) return position;
        
        position += Vector2.up * edgeDistance;
        return position;
    }
}