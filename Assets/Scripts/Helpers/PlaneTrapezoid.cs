using UnityEngine;

public class PlaneTrapezoid
{
    // { bottomLeft, bottomRight, topLeft, topRight }
    public Vector2[] points { get; private set; }
    public float y { get; private set; }

    private PlaneTriangle areaABC;
    private PlaneTriangle areaACD;

    public PlaneTrapezoid(Vector2 bottomLeft, Vector2 bottomRight, Vector2 topLeft, Vector2 topRight, float y = 0f)
    {
        points = new Vector2[4] { bottomLeft, bottomRight, topLeft, topRight };
        this.y = y; Initialize();
    }
    public PlaneTrapezoid(Vector2[] points, float y = 0f) { this.points = points; this.y = y; Initialize(); }
    private void Initialize()
    {
        areaABC = new PlaneTriangle(points[0], points[1], points[3]);
        areaACD = new PlaneTriangle(points[0], points[3], points[2]);
    }



    public float GetWidth()
    {
        float min = Mathf.Infinity;
        float max = -Mathf.Infinity;
        for (int i = 0; i < points.Length; ++i)
        {
            if (points[i].x < min) { min = points[i].x; }
            if (points[i].x > max) { max = points[i].x; }
        }
        return max - min;
    }


    public bool IsInside(Vector2 point)
    {
        return areaABC.IsInside(point) || areaACD.IsInside(point);
    }
    public Vector2 SamplePoint()
    {
        float totalArea = areaABC.Area() + areaACD.Area();
        Vector3 point;
        // Pick triangle proportional to its area
        if (Random.value < areaABC.Area() / totalArea) { point = areaABC.SamplePoint(); }
        else { point = areaACD.SamplePoint(); }
        return point;
    }
}

public class PlaneTriangle
{
    public Vector2[] points { get; private set; }
    public float y { get; private set; }

    public PlaneTriangle(Vector2 a, Vector2 b, Vector2 c, float y = 0f)
    {
        points = new Vector2[3] { a, b, c };
        this.y = y;
    }


    public float Area()
    {
        return Mathf.Abs(
            (points[1].x - points[0].x) * (points[2].y - points[0].y) -
            (points[2].x - points[0].x) * (points[1].y - points[0].y)
        ) * 0.5f;
    }
    public bool IsInside(Vector2 point)
    {
        float d1 = Cross(points[1] - points[0], point - points[0]);
        float d2 = Cross(points[2] - points[1], point - points[1]);
        float d3 = Cross(points[0] - points[2], point - points[2]);

        bool hasNegative = (d1 < 0) || (d2 < 0) || (d3 < 0);
        bool hasPositive = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(hasNegative && hasPositive);
    }
    public Vector3 SamplePoint()
    {
        float r1 = Mathf.Sqrt(Random.value);
        float r2 = Random.value;

        return points[0] +
               r1 * (1 - r2) * (points[1] - points[0]) +
               r1 * r2 * (points[2] - points[0]);
    }


    float Cross(Vector2 a, Vector2 b) { return a.x * b.y - a.y * b.x; }
}
