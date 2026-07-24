using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMapBounds : MonoBehaviour
{
    private Camera camera;
    [SerializeField] private GameObject waterPlane;

    public static Vector2 boundsMin { get; private set; }
    public static Vector2 boundsMax { get; private set; }

    [SerializeField] private GameObject boundsVisualGO;
    [SerializeField] private bool visualizeOnStart = false;


    private void Awake() 
    {
        camera = GetComponent<Camera>();
        boundsVisualGO.SetActive(false);
    }
    private void Start()
    {
        CalculateBounds();
        if (visualizeOnStart) { VisualizeBounds(boundsMin, boundsMax); }
    }



    void CalculateBounds()
    {
        float distance = Mathf.Abs(transform.position.y - waterPlane.transform.position.y);

        float visibleHeight = 2f * distance *
                      Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad * 0.5f);

        float visibleWidth = visibleHeight * camera.aspect;

        Vector2 center = new Vector3(
            camera.transform.position.x,
            camera.transform.position.z
        );

        boundsMin = center + new Vector2(-visibleWidth / 2f, -visibleHeight / 2f);
        boundsMax = center + new Vector2(visibleWidth / 2f, visibleHeight / 2f);
    }

    void VisualizeBounds(Vector2 min, Vector2 max)
    {
        boundsVisualGO.transform.position = new Vector3(transform.position.x, waterPlane.transform.position.y + 0.01f, transform.position.z);
        Vector2 size = new Vector2(max.x - min.x, max.y - min.y);
        boundsVisualGO.transform.localScale = new Vector3(size.x, size.y, 1.0f);

        boundsVisualGO.SetActive(true);
    }
}
