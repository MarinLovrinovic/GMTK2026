using UnityEngine;

public class NuclearBomb : Bomb
{
    [SerializeField] private float finalExplosionRadius;

    protected override void preExplosionLogic()
    {
        explosionRadius = finalExplosionRadius;
    }

    protected override void explosionLogic(IHittable hittable, bool isSelfCaused)
    {
        if (isSelfCaused) hittable.Hit(damage);
    }
}
