using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private Bomb[] bombs;
    [SerializeField] private float[] weightedProbabilities;

    private int totalTimer = 0;
    private int timeUntilNextBomb = 2;

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
        totalTimer++;
        timeUntilNextBomb--;

        if (timeUntilNextBomb <= 0)
        {
            Vector2 location = new Vector2(Random.Range(-7f, 7f), Random.Range(-3.5f, 3.5f)) * Scaler.Scale;
            Bomb bombType = MathHelper.WeightedRandomFromDistributionArray<Bomb>(bombs, weightedProbabilities);
            Instantiate(bombType, location.xoy(), Quaternion.identity);

            timeUntilNextBomb = Random.Range(1, 1 + Mathf.Min(6, 60 / totalTimer));
        }
    }
}