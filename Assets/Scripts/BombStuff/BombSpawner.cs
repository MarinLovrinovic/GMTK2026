using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private Bomb[] bombs;
    [SerializeField] private float[] weightedProbabilities;
    
    private int totalTimer = 0;
    private int timeUntilNextBomb = 2;
    private List<Bomb> deployedBombs = new();
    
    private void OnEnable()
    {
        TickDriver.instance.OnTick += Tick;
    }

    private void OnDisable()
    {
        TickDriver.instance.OnTick -= Tick;
    }

    void Tick()
    {
        deployedBombs.RemoveAll(bomb => !bomb);
        
        totalTimer++;
        timeUntilNextBomb--;

        if (timeUntilNextBomb <= 0)
        {
            SpawnBomb();
            timeUntilNextBomb =
                MathHelper.ProbabilisticRoundToInt(Random.Range(1f, 1f + Mathf.Min(6f, 60f / totalTimer)));
        }
    }

    private void SpawnBomb()
    {
        Vector2 location = new Vector2(Random.Range(-12f, 12f), Random.Range(-2f, 9f)) * Scaler.Scale;
        
        // 40 attempts to find a location that does not overlap with other bombs,
        // otherwise cancel spawn
        bool bombsAtLocation = true;
        for (int i = 0; i < 40; i++) 
        {
            if (!BombsAtLocation(location))
            {
                bombsAtLocation = false;
                break;
            }
            location = CameraMapBounds.activeArea.SamplePoint();
            location *= 0.2f;
        }

        if (bombsAtLocation)
        {
            return;
        }
        
        Bomb bombType = MathHelper.WeightedRandomFromDistributionArray(bombs, weightedProbabilities);
        Bomb spawnedBomb = Instantiate(bombType, location.xoy(), Quaternion.identity);
        deployedBombs.Add(spawnedBomb);
    }

    private bool BombsAtLocation(Vector2 location)
    {
        foreach (Bomb deployedBomb in deployedBombs)
        {
            if (Vector2.Distance(deployedBomb.Position, location) <= deployedBomb.getExplosionRadius())
            {
                return true;
            }
        }
        return false;
    }
    
    /*
    private float totalTimer = 0;
    [SerializeField] private Bomb[] bombs;
    [SerializeField] private float[] weightedProbabilities;
    private float timeUntilNextBomb = 2;
    private void Update()
    {
        totalTimer += Time.deltaTime;
        timeUntilNextBomb -= Time.deltaTime;
        if (timeUntilNextBomb <= 0)
        {
            Vector2 location = new Vector2(Random.Range(-7f, 7f), Random.Range(-3.5f, 3.5f)) * Scaler.Scale;
            Bomb bombType = MathHelper.WeightedRandomFromDistributionArray<Bomb>(bombs, weightedProbabilities);
            Instantiate(bombType, location.xoy(), Quaternion.identity);

            timeUntilNextBomb = Random.Range(1f, 1f + Mathf.Min(6, 60 / totalTimer));
        }
    }
    */
}