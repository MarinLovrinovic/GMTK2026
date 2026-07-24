using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private Vehicle vehicle;
    private float timeUntilNextVehicle = 1;
    private void Update()
    {
        timeUntilNextVehicle -= Time.deltaTime;
        if (timeUntilNextVehicle <= 0)
        {
            
            Vector2 location = GetInitialVehicleLocation();
            Vector2 velocity = -location.normalized.RandomVectorDeviation(40);
            
            Vehicle newVehicle = Instantiate(vehicle, location.xoy(), velocity.Vector2ToQuaternion());
            newVehicle.velocity = velocity;
            
            timeUntilNextVehicle = Random.Range(0.2f, 2f);
        }
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