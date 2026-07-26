using UnityEngine;

public class ChargeBomb : Bomb
{
    protected override void perTickLogic()
    {
        explosionRadius++;
        UpdateDisplayRadius(explosionRadius);
    }
}
