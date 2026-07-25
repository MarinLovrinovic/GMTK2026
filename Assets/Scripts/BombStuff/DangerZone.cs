using UnityEngine;

public class DangerZone : MonoBehaviour
{
    [SerializeField] private float fireDamage;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLLISION");

        IHittable hittable = other.gameObject.GetComponent<IHittable>();

        if (hittable != null)
        {
            hittable.Hit(fireDamage);
        }
    }
}
