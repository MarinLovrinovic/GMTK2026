using UnityEngine;

public class BombDropper : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private int dropIntervalInTicks;

    private int ticksPassed;

    private void Start()
    {
        ticksPassed = 0;
    }

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
        ticksPassed++;

        if (ticksPassed >= dropIntervalInTicks)
        {
            Instantiate(bombPrefab, gameObject.transform.position, Quaternion.identity);
            ticksPassed = 0;
        }
    }
}
