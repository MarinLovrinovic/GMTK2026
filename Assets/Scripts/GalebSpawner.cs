using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GalebSpawner : MonoBehaviour
{
    [SerializeField] private Galeb galebPrefab;
    private List<Galeb> galebi = new List<Galeb>();

    //[SerializeField] private Vector2 heightRange;
    [SerializeField] private float height = 2f;
    [SerializeField] private Vector2 scaleRange;

    [SerializeField] private Vector2 spawnTimeRange;
    private float timeUntilNextGaleb = 10;

    [SerializeField] private Vector2 speedRange;

    private void Start()
    {
        timeUntilNextGaleb = Random.Range(spawnTimeRange.x, spawnTimeRange.y);
    }

    private void Update()
    {
        galebi.RemoveAll(galeb =>
        {
            if (!galeb) return true;

            Vector3 pos = galeb.transform.position;
            if (!CameraMapBounds.activeArea.IsInside(new Vector2(pos.x, pos.z), 10f)
                || pos.y < -2f)
            {
                Destroy(galeb.gameObject);
                return true;
            }

            return false;
        });

        timeUntilNextGaleb -= Time.deltaTime;
        if (timeUntilNextGaleb <= 0)
        {
            SpawnGaleb();
            timeUntilNextGaleb = Random.Range(spawnTimeRange.x, spawnTimeRange.y);
        }
    }


    private void SpawnGaleb()
    {
        Vector2 newGalebPositionXZ = CameraMapBounds.activeArea.SampleEdgePoint(out Vector2 normal);
        Vector3 newGalebPosition = new Vector3(newGalebPositionXZ.x, height, newGalebPositionXZ.y);  //Random.Range(heightRange.x, heightRange.y)

        Galeb newGaleb = Instantiate(galebPrefab, newGalebPosition, Quaternion.identity);

        newGaleb.speed = Random.Range(speedRange.x, speedRange.y);
        Vector2 newGalebVelocity = normal.RandomVectorDeviation(30f) * newGaleb.speed;

        float newGalebScale = Random.Range(scaleRange.x, scaleRange.y);

        newGaleb.transform.position = newGalebPosition;
        newGaleb.velocity = newGalebVelocity;
        newGaleb.transform.localScale = new Vector3(newGalebScale, newGalebScale, newGalebScale);
        Debug.DrawRay(newGalebPosition, newGalebVelocity.xoy().normalized * 6f, Color.pink, 10f);

        galebi.Add(newGaleb);
    }
}
