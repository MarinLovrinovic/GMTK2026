using System.Collections;
using UnityEngine;

public class FreezeBomb : Bomb
{
    [SerializeField] private float freezeTime;
    [SerializeField] private GameObject icePrefab;

    public static bool freezeShip = false;

    protected override void preExplosionLogic(bool isSelfCaused)
    {
        ServiceProvider.Instance.soundManager.playSound(2);
    }

    protected override void explosionLogic(IHittable hittable, bool isSelfCaused)
    {
        hittable.Freeze(freezeTime);
    }
}
