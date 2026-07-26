using System;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(Camera))]
public class CameraMapBounds : MonoBehaviour
{
    private static CameraMapBounds Instance;
    private Camera camera;
    [SerializeField] private GameObject waterGO;

    public static PlaneTrapezoid activeArea { get; private set; }

    [SerializeField] private bool visualize = false;
    [SerializeField] private Material visualizationMaterial;
    [SerializeField] private float visualHeight = 0.1f;
    private GameObject visualGO = null;


    private void Awake() 
    {
        if (Instance != null) { Debug.LogError("Multiple CameraMapBounds scripts in scene."); }
        Instance = this;
        camera = GetComponent<Camera>();
    }
    private void Start()
    {
        CalculateBounds();
        if (visualize) { VisualizeBounds(activeArea); }
    }


    public static void UpdateBounds()
    {
        Instance.CalculateBounds();
        if (Instance.visualize) { Instance.VisualizeBounds(activeArea); }
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

    public bool BombAtLeftEdge()
    {


        return false;
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
        visualGO.transform.position = new Vector3(0f, visualHeight, 0f);
    }
}