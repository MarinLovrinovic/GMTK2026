using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private Vehicle vehiclePrefab;
    private float timeUntilNextVehicle = 1;
    private List<Vehicle> vehicles = new();
    
    private void Update()
    {
        vehicles.RemoveAll(vehicle =>
        {
            if (!vehicle) return true;
            
            Vector2 pos = vehicle.Position;
            if (Mathf.Abs(pos.x) > 10 * Scaler.Scale || Mathf.Abs(pos.y) > 6 * Scaler.Scale) // vehicle out of bounds
            {
                Destroy(vehicle.gameObject);
                return true;
            }
            return false;
        });

        foreach (Vehicle vehicle in vehicles)
        {
            float timeUntilCollision = TimeUntilCollision(vehicle.Position, vehicle.velocity, vehicle.radius, vehicle);
            if (timeUntilCollision < 2.5f)
            {
                if (vehicle.turningDirection == 0)
                {
                    vehicle.turningDirection = MathHelper.RandomBool() ? 1 : -1;
                }
                vehicle.velocity = vehicle.velocity.RotateVector2ByDegrees(vehicle.turningDirection * vehicle.turningSpeed * Time.deltaTime);
            }
            else
            {
                vehicle.turningDirection = 0;
            }
        }
        
        timeUntilNextVehicle -= Time.deltaTime;
        if (timeUntilNextVehicle <= 0)
        {
            SpawnVehicle();
            timeUntilNextVehicle = 0.3f; // Random.Range(0.2f, 2f);
        }
    }

    private void SpawnVehicle()
    {
        Vector2 newVehiclePosition = GetInitialVehicleLocation(out Vector2 normal);
            
        Vehicle newVehicle = Instantiate(vehiclePrefab, newVehiclePosition.xoy(), Quaternion.identity);

        Vector2 newVehicleVelocity = normal.RandomVectorDeviation(80) * newVehicle.speed;

        // 40 attempts to find a path which does not collide with any other vehicles,
        // otherwise cancel spawn
        for (int i = 0; i < 40; i++)
        {
            if (!CollisionOnPath(newVehiclePosition, newVehicleVelocity, newVehicle.radius))
            {
                break;
            }
            newVehiclePosition = GetInitialVehicleLocation(out normal);
            newVehicleVelocity = normal.RandomVectorDeviation(80) * newVehicle.speed;
            
            if (i == 39)
            {
                Debug.Log("failed to find path");
                Destroy(newVehicle.gameObject);
                return;
            }
        }

        newVehicle.transform.position = newVehiclePosition.xoy();
        newVehicle.velocity = newVehicleVelocity;

        vehicles.Add(newVehicle);
    }
    
    private float TimeUntilCollision(Vector2 position, Vector2 velocity, float radius, Vehicle ignore = null)
    {
        float minTimeUntilCollision = Mathf.Infinity;
        foreach (Vehicle otherVehicle in vehicles)
        {
            if (ignore == otherVehicle) continue;
            
            minTimeUntilCollision = Mathf.Min(minTimeUntilCollision, MathHelper.MovingCirclesCollisionTime(
                position, velocity, radius,
                otherVehicle.Position, otherVehicle.velocity, otherVehicle.radius));
        }
        return minTimeUntilCollision;
    }
    
    private bool CollisionOnPath(Vector2 position, Vector2 velocity, float radius, Vehicle ignore = null)
    {
        bool anyCollisions = false;
        foreach (Vehicle otherVehicle in vehicles)
        {
            if (ignore == otherVehicle) continue;
            
            anyCollisions |= MathHelper.MovingCirclesIntersect(
                position, velocity, radius,
                otherVehicle.Position, otherVehicle.velocity, otherVehicle.radius);
        }
        return anyCollisions;
    }

    private Vector2 GetInitialVehicleLocation(out Vector2 normal)
    {
        float edgeDistance = Random.Range(0f, (2f * 18f + 2f * 11f) * Scaler.Scale);
        Vector2 position = new Vector2(-9f, 5.5f) * Scaler.Scale;

        position += Vector2.right * Mathf.Clamp(edgeDistance, 0, 18f * Scaler.Scale);
        edgeDistance -= 18f * Scaler.Scale;
        if (edgeDistance <= 0f)
        {
            normal = Vector2.down;
            return position;
        }

        position += Vector2.down * Mathf.Clamp(edgeDistance, 0, 11f * Scaler.Scale);
        edgeDistance -= 11f * Scaler.Scale;
        if (edgeDistance <= 0f)
        {
            normal = Vector2.left;
            return position;
        }

        position += Vector2.left * Mathf.Clamp(edgeDistance, 0, 18f * Scaler.Scale);
        edgeDistance -= 18f * Scaler.Scale;
        if (edgeDistance <= 0f)
        {
            normal = Vector2.up;
            return position;
        }
        
        position += Vector2.up * edgeDistance;
        normal = Vector2.right;
        return position;
    }
    
    // private void SpawnVehicle()
    // {
    //     Vector2 newVehiclePosition = GetInitialVehicleLocation(out Vector2 normal);
    //         
    //     Vehicle newVehicle = Instantiate(vehicle, newVehiclePosition.xoy(), Quaternion.identity);
    //
    //     int randomDeviationAngle = Random.Range(-80, 80);
    //     
    //     Vector2? velocity = normal.RotateVector2ByDegrees(randomDeviationAngle) * newVehicle.speed;
    //     
    //     if (CollisionOnPath(newVehiclePosition, velocity.Value, newVehicle.radius))
    //     {
    //         if (randomDeviationAngle >= 0)
    //         {
    //             velocity = SearchPath(newVehiclePosition,  newVehicle.radius, newVehicle.speed,  normal, randomDeviationAngle, -1)
    //                        ?? SearchPath(newVehiclePosition,  newVehicle.radius, newVehicle.speed, normal, randomDeviationAngle, 1);
    //         }
    //         else
    //         {
    //             velocity = SearchPath(newVehiclePosition,  newVehicle.radius, newVehicle.speed, normal, randomDeviationAngle, 1)
    //                        ?? SearchPath(newVehiclePosition,  newVehicle.radius, newVehicle.speed,  normal, randomDeviationAngle, -1);
    //         }
    //
    //         if (!velocity.HasValue)
    //         {
    //             Debug.Log("failed to find path");
    //         }
    //     }
    //     
    //     newVehicle.velocity = velocity ?? normal;
    //
    //     vehicles.Add(newVehicle);
    // }
    //
    // private Vector2? SearchPath(Vector2 vehiclePosition, float vehicleRadius, float vehicleSpeed,
    //     Vector2 normal, int startingDeviationAngle, int step)
    // {
    //     for (int angle = startingDeviationAngle; step < 0 ? -85 < angle : angle < 85 ; angle += step)
    //     {
    //         Vector2 velocity = normal.RotateVector2ByDegrees(angle) * vehicleSpeed;
    //         if (!CollisionOnPath(vehiclePosition, velocity, vehicleRadius))
    //         {
    //             return velocity;
    //         }
    //     }
    //     return null;
    // }
}