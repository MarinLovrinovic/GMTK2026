using System;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(Camera))]
public class CameraMapBounds : MonoBehaviour
{
    private Camera camera;
    [SerializeField] private GameObject waterGO;

    public static PlaneTrapezoid activeArea { get; private set; }

    [SerializeField] private bool visualizeOnStart = false;
    [SerializeField] private Material visualizationMaterial;
    private GameObject visualGO = null;


    private void Awake() 
    {
        camera = GetComponent<Camera>();
    }
    private void Start()
    {
        CalculateBounds();
        if (visualizeOnStart) { VisualizeBounds(activeArea); }
    }


    void CalculateBounds()
    {
        Plane waterPlane = new Plane(waterGO.transform.up, waterGO.transform.position);
        Vector3[] viewportCorners =
        {
            new Vector3(0, 0, 0), // bottom-left
            new Vector3(1, 0, 0), // bottom-right
            new Vector3(0, 1, 0), // top-left
            new Vector3(1, 1, 0)  // top-right
        };
        Vector2[] bounds = new Vector2[4];
        for (int i = 0; i < 4; ++i)
        {
            Vector3 corner = viewportCorners[i];
            Ray ray = camera.ViewportPointToRay(corner);
            if (waterPlane.Raycast(ray, out float distance))
            {
                Vector3 worldPoint = ray.GetPoint(distance);
                bounds[i] = new Vector2(worldPoint.x, worldPoint.z);
            }
        }
        activeArea = new PlaneTrapezoid(bounds, waterGO.transform.position.y);
    }

    void VisualizeBounds(PlaneTrapezoid activeArea)
    {
        if (visualGO != null) { Destroy(visualGO); }
        Mesh mesh = new Mesh();
        mesh.name = "BoundsVisualization";
        Vector3[] vertices = new Vector3[4];
        for (int i = 0; i < 4; ++i)
        {
            vertices[i] = new Vector3(activeArea.points[i].x, 0f, activeArea.points[i].y);
        }
        int[] triangles = new int[]
        {
            2, 1, 0,
            2, 3, 1
        };
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        // Optional: calculate normals for lighting
        mesh.RecalculateNormals();
        // Assign mesh to object
        visualGO = new GameObject("BoundsVisualization");
        MeshFilter filter = visualGO.AddComponent<MeshFilter>();
        MeshRenderer renderer = visualGO.AddComponent<MeshRenderer>();
        filter.mesh = mesh;
        renderer.material = visualizationMaterial;
        Instantiate(visualGO, new Vector3(0f, 0.1f, 0f), Quaternion.identity, transform);
    }
}