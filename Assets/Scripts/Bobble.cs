using UnityEngine;

public class Bobble : MonoBehaviour
{
    private void LateUpdate()
    {
        // Height
        float height = SeaPlane.Instance.SampleHeight(transform.position);
        transform.position = new Vector3(transform.position.x, height, transform.position.z);

        // Normal
        float e = 0.1f;
        float hL = SeaPlane.Instance.SampleHeight(transform.position + Vector3.left * e);    // x - e
        float hR = SeaPlane.Instance.SampleHeight(transform.position + Vector3.right * e);    // x + e
        float hB = SeaPlane.Instance.SampleHeight(transform.position + Vector3.back * e);    // z - e
        float hF = SeaPlane.Instance.SampleHeight(transform.position + Vector3.forward * e);    // z + e
        Vector3 normal = new Vector3(
            hL - hR,
            2f * e,
            hB - hF
        ).normalized;
        transform.rotation = Quaternion.LookRotation(transform.forward, normal);
    }
}
