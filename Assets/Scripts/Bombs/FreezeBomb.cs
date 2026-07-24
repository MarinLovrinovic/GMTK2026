using UnityEngine;

public class FreezeBomb : Bomb
{
    [SerializeField] private float freezeTime;

    protected override void explosionLogic(IHittable hittable)
    {
        hittable.Freeze(freezeTime);
    }
}
