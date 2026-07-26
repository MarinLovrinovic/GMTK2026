using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private Vehicle vehiclePrefab;
    private float timeUntilNextVehicle = 1;
    private List<Vehicle> vehicles = new();

    [SerializeField, Range(15f, 90f)] private float spawnVelMaxAngle;
    [SerializeField] private float leaveBoundsPadding = 0f;

    private void Update()
    {
        vehicles.RemoveAll(vehicle =>
        {
            if (!vehicle) return true;

            Vector2 pos = vehicle.Position;
            if (!CameraMapBounds.activeArea.IsInside(pos, leaveBoundsPadding))
            {
                Destroy(vehicle.gameObject);
                return true;
            }

            return false;
        });

        foreach (Vehicle vehicle in vehicles)
        {
            Vehicle eminentlyCollidingVehicle =
                EminentCollision(vehicle.Position, vehicle.velocity, vehicle.radius, vehicle);
            if (eminentlyCollidingVehicle != null)
            {
                vehicle.turningDirection =
                    CalculateTurnDirection(vehicle.Position, vehicle.velocity, eminentlyCollidingVehicle.Position);
                continue;
            }
            
            (float timeUntilCollision, Vehicle offendingVehicle) =
                TimeUntilCollision(vehicle.Position, vehicle.velocity, vehicle.radius, vehicle);
            if (timeUntilCollision < 2f)
            {
                vehicle.turningDirection =
                    CalculateTurnDirection(vehicle.Position, vehicle.velocity, offendingVehicle.Position);
                continue;
            }
            
            vehicle.turningDirection = 0;
        }
        
        timeUntilNextVehicle -= Time.deltaTime;
        if (timeUntilNextVehicle <= 0)
        {
            SpawnVehicle();
            timeUntilNextVehicle = Random.Range(0.2f, 2f);
        }
    }

    private void SpawnVehicle()
    {
        Vector2 newVehiclePosition = GetInitialVehicleLocation(out Vector2 normal);
            
        Vehicle newVehicle = Instantiate(vehiclePrefab, newVehiclePosition.xoy(), Quaternion.identity);

        Vector2 newVehicleVelocity = normal.RandomVectorDeviation(80) * newVehicle.speed;

        // 40 attempts to find a path which does not collide with any other vehicles,
        // otherwise cancel spawn
        bool collisionOnPath = true;
        for (int i = 0; i < 40; i++)
        {
            if (!CollisionOnPath(newVehiclePosition, newVehicleVelocity, newVehicle.radius))
            {
                collisionOnPath = false;
                break;
            }
            newVehiclePosition = GetInitialVehicleLocation(out normal);
            newVehicleVelocity = normal.RandomVectorDeviation(spawnVelMaxAngle) * newVehicle.speed;
        }

        if (collisionOnPath)
        {
            Destroy(newVehicle.gameObject);
            return;
        }

        newVehicle.transform.position = newVehiclePosition.xoy();
        newVehicle.velocity = newVehicleVelocity;
        Debug.DrawRay(newVehiclePosition.xoy(), newVehicleVelocity.xoy().normalized * 6f, Color.purple, 10f);

        vehicles.Add(newVehicle);
    }

    private float CalculateTurnDirection(Vector2 position, Vector2 velocity, Vector2 obstaclePosition)
    {
        Vector2 toObstacle = obstaclePosition - position;
        return Mathf.Sign(Vector2.SignedAngle(toObstacle, velocity));
    }
    
    private bool ObstacleInFront(Vector2 position, Vector2 velocity, Vector2 obstaclePosition)
    {
        Vector2 toObstacle = obstaclePosition - position;
        return 0f < Vector2.Dot(toObstacle, velocity);
    }

    [CanBeNull] private Vehicle EminentCollision(Vector2 position, Vector2 velocity, float radius, Vehicle ignore = null)
    {
        float minDistance = Mathf.Infinity;
        Vehicle closestVehicle = null;
        foreach (Vehicle otherVehicle in vehicles)
        {
            if (ignore == otherVehicle) continue;

            float distance = Vector2.Distance(position, otherVehicle.Position);
            
            // we are in another vehicle's radius
            if (distance < radius + otherVehicle.radius)
            {
                // we are moving toward the vehicle
                if (ObstacleInFront(position, velocity, otherVehicle.Position))
                {
                    // it is the closest we have found so far
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestVehicle = otherVehicle;
                    }
                }
            }
        }
        return closestVehicle;
    }

    private (float time, Vehicle vehicle) TimeUntilCollision(Vector2 position, Vector2 velocity, float radius, Vehicle ignore = null)
    {
        float minTimeUntilCollision = Mathf.Infinity;
        Vehicle offendingVehicle = null;
        foreach (Vehicle otherVehicle in vehicles)
        {
            if (ignore == otherVehicle) continue;

            float timeUntilCollision = MathHelper.MovingCirclesCollisionTime(
                position, velocity, radius,
                otherVehicle.Position, otherVehicle.velocity, otherVehicle.radius);
            if (timeUntilCollision < minTimeUntilCollision)
            {
                minTimeUntilCollision = timeUntilCollision;
                offendingVehicle = otherVehicle;
            }
        }

        return (minTimeUntilCollision, offendingVehicle);
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
        return CameraMapBounds.activeArea.SampleEdgePoint(out normal);
    }
    /*
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
    }*/
}