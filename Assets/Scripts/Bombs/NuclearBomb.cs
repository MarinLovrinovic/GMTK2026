using UnityEngine;

public class NuclearBomb : Bomb
{
    [SerializeField] private float finalExplosionRadius;

    protected override void preExplosionLogic(bool isSelfCaused)
    {
        ServiceProvider.Instance.soundManager.playSound(3);

        if (isSelfCaused)
        {
            explosionRadius = finalExplosionRadius;
        }
    }
}
