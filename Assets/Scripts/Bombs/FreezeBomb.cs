using System.Collections;
using UnityEngine;

public class FreezeBomb : Bomb
{
    [SerializeField] private float freezeTime;
    [SerializeField] private GameObject icePrefab;

    public static bool freezeShip = false;

    protected override void explosionLogic(IHittable hittable, bool isSelfCaused)
    {
        hittable.Freeze(freezeTime);
    }
}
