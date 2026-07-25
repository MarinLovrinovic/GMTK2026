using UnityEngine;

public class FreezeBomb : Bomb
{
    [SerializeField] private float freezeTime;

    protected override void explosionLogic(IHittable hittable, bool isSelfCaused)
    {
        hittable.Freeze(freezeTime);
    }
}
