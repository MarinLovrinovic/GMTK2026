using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float height;
    public float radius;
    public float scaleToRadiusFactor = 1f;
    public Vector2 Position => transform.position.xz();

}
