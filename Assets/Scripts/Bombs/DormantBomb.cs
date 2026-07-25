using UnityEngine;

public class DormantBomb : Bomb
{
    [SerializeField] private float finalExplosionRadius;

    protected override void preExplosionLogic()
    {
        explosionRadius = finalExplosionRadius;
    }
}
