using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMapBounds : MonoBehaviour
{
    private Camera camera;
    [SerializeField] private GameObject waterGO;

    public static Vector2 boundsMin { get; private set; }
    public static Vector2 boundsMax { get; private set; }

    [SerializeField] private GameObject boundsVisualGO;
    [SerializeField] private bool visualizeOnStart = false;


    private void Awake() 
    {
        camera = GetComponent<Camera>();
        boundsVisualGO.SetActive(false);
        boundsVisualGO.transform.rotation = Quaternion.identity;
    }
    private void Start()
    {
        CalculateBoundsRaycast();
        if (visualizeOnStart) { VisualizeBounds(boundsMin, boundsMax); }
    }



    void CalculateBounds()
    {
        float distance = Mathf.Abs(transform.position.y - waterGO.transform.position.y);

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
    void CalculateBoundsRaycast()
    {
        Plane waterPlane = new Plane(waterGO.transform.up, waterGO.transform.position);

        Vector3[] viewportCorners =
        {
            new Vector3(0, 0, 0), // bottom-left
            new Vector3(1, 0, 0), // bottom-right
            new Vector3(0, 1, 0), // top-left
            new Vector3(1, 1, 0)  // top-right
        };

        Vector3 min = Vector3.one * float.MaxValue;
        Vector3 max = Vector3.one * float.MinValue;

        foreach (Vector3 corner in viewportCorners)
        {
            Ray ray = camera.ViewportPointToRay(corner);

            if (waterPlane.Raycast(ray, out float distance))
            {
                Vector3 worldPoint = ray.GetPoint(distance);
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = worldPoint;

                min = Vector3.Min(min, worldPoint);
                max = Vector3.Max(max, worldPoint);
            }
        }

        boundsMin = new Vector2(min.x, min.z);
        boundsMax = new Vector2(max.x, max.z);
    }

    void VisualizeBounds(Vector2 min, Vector2 max)
    {
        Vector2 size = new Vector2(max.x - min.x, max.y - min.y);
        boundsVisualGO.transform.position = new Vector3(min.x + (size.x / 2f), waterGO.transform.position.y + 0.01f, min.y + (size.y / 2f));
        boundsVisualGO.transform.localScale = new Vector3(size.x, 1.0f, size.y);

        boundsVisualGO.SetActive(true);
    }
}
