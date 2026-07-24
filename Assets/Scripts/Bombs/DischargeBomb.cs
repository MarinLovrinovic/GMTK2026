using UnityEngine;

public class DischargeBomb : Bomb
{
    protected override void perTickLogic()
    {
        explosionRadius--;
    }
}
