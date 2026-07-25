using UnityEngine;

public class NapalmBomb : Bomb
{
    [SerializeField] private GameObject napalmFieldPrefab;
    [SerializeField] private float napalmFieldLifetime;
    [SerializeField] private Vector3 napalmFieldScale;

    protected override void explosionLogic(IHittable hittable, bool isSelfCaused)
    {
        GameObject napalmField = Instantiate(napalmFieldPrefab, gameObject.transform.position, Quaternion.identity);
        napalmField.transform.localScale = napalmFieldScale;
        Destroy(napalmField, napalmFieldLifetime);
        hittable.Hit(damage);
    }
}
