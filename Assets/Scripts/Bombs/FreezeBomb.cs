using System.Collections;
using UnityEngine;

public class FreezeBomb : Bomb
{
    [SerializeField] private float freezeTime;
    [SerializeField] private GameObject icePrefab;

    private Vector3 localOffset = new Vector3(0.0f, 0.4f, 0.1f);

    public static bool freezeShip = false;

    protected override void explosionLogic(IHittable hittable, bool isSelfCaused)
    {
        hittable.Freeze(freezeTime);
        
        /*if (hittable.GetType() == typeof(Vehicle))
        {
            // Spawn -> obsolete
            //GameObject iceCube = Instantiate(icePrefab, ((Vehicle)hittable).gameObject.transform);
            //iceCube.transform.localPosition = localOffset;
            //Destroy(iceCube, freezeTime);
        }*/
    }
}
