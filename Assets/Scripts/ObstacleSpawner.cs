using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private int obstacleCount = 3;
    [SerializeField] private int tileCount = 20;


    private void Start()
    {
        
    }


    Vector2 GetRandomPosition()
    {


        return Vector2.zero;
    }

    void SpawnObstacle(Vector2 position)
    {

    }
}
