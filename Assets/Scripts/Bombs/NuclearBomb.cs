using UnityEngine;

public class NuclearBomb : Bomb
{
    [SerializeField] private float finalExplosionRadius;

    protected override void preExplosionLogic(bool isSelfCaused)
    {
        if (isSelfCaused)
        {
            explosionRadius = finalExplosionRadius;
        }
    }
}
