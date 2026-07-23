using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    private float totalTimer = 0;
    [SerializeField] private Bomb bomb;
    private float timeUntilNextBomb = 2;
    private void Update()
    {
        totalTimer += Time.deltaTime;
        timeUntilNextBomb -= Time.deltaTime;
        if (timeUntilNextBomb <= 0)
        {
            Vector2 location = new Vector2(Random.Range(-7f, 7f), Random.Range(-3.5f, 3.5f)) * Scaler.Scale;
            Instantiate(bomb, location.xyo(), Quaternion.identity);
            
            timeUntilNextBomb = Random.Range(1f, 1f + Mathf.Min(6, 60 / totalTimer));
        }
    }
}