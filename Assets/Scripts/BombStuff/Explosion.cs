using UnityEngine;

public class Explosion : MonoBehaviour
{
    void Start()
    {
        doEffects();
        Destroy(gameObject, 1);
    }

    protected virtual void doEffects() { }
}
