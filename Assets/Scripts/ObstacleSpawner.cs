using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Obstacle[] obstaclePrefabs;
    [SerializeField] private int obstacleCount = 3;
    [SerializeField] private float minimumDistance = 10f;

    [SerializeField] private Vector2 radiusRange;

    public List<Obstacle> obstacles { get; private set; }

    private CameraMovement cameraMovement;


    private void Start()
    {
        obstacles = new();
        FillArea(CameraMapBounds.activeArea);

        cameraMovement = FindAnyObjectByType<CameraMovement>();
    }


    private void Update()
    {
        if (!cameraMovement.transitioning)
        {
            obstacles.RemoveAll(obstacle =>
            {
                if (!obstacle) return true;

                Vector2 pos = obstacle.Position;
                if (!CameraMapBounds.activeArea.IsInside(pos))
                {
                    Destroy(obstacle.gameObject);
                    return true;
                }

                return false;
            });
        }
    }

    public void FillArea(PlaneTrapezoid activeArea, int obstacleCount = 0)
    {
        if (obstacleCount == 0) { obstacleCount = this.obstacleCount; }
        for (int i = 0; i < obstacleCount; ++i) { SpawnObstacle(activeArea); }
    }

    void SpawnObstacle(PlaneTrapezoid activeArea)
    {
        Obstacle prefab = GetRandomPrefab();
        float newObstacleRadius = Random.Range(radiusRange.x, radiusRange.y);
        Vector2? newObstaclePositionXZ = GetRandomPosition(activeArea, newObstacleRadius);
        if (!newObstaclePositionXZ.HasValue) { return; }
        Vector3 newObstaclePosition = new Vector3(newObstaclePositionXZ.Value.x, prefab.height, newObstaclePositionXZ.Value.y);
        Quaternion newObstacleRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        float newObstacleScale = (newObstacleRadius / prefab.scaleToRadiusFactor) / 2f;

        Obstacle newObstacle = Instantiate(prefab, newObstaclePosition, newObstacleRotation);
        newObstacle.transform.localScale = new Vector3(newObstacleScale, newObstacleScale, newObstacleScale);
        newObstacle.radius = newObstacleRadius;

        obstacles.Add(newObstacle);
    }

    Vector2? GetRandomPosition(PlaneTrapezoid activeArea, float radius)
    {
        Vector2 position = activeArea.SamplePoint();
        for (int i = 0; i < 40; ++i)
        {
            bool clear = true;
            // Check if far away from edges
            if (activeArea.MinDistance(position) < minimumDistance) { clear = false; }
            if (clear)
            {
                // Check if far away from existing
                foreach (Obstacle obstacle in obstacles)
                {
                    float distance = Vector2.Distance(position, obstacle.Position) - obstacle.radius - radius;
                    if (distance < minimumDistance) { clear = false; break; }
                }
                if (clear) { return position; }
            }
            position = activeArea.SamplePoint();
        }
        Debug.LogWarning("Failed to find a free place for next obstacle.");
        return null;
    }
    Obstacle GetRandomPrefab()
    {
        return obstaclePrefabs[(int)Random.Range(0, obstaclePrefabs.Length)];
    }
}
