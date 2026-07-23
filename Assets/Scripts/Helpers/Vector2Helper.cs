using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Vector2Helper
{
    public static Quaternion Vector2ToQuaternion(this Vector2 vector2)
    {
        float angle = Vector2.SignedAngle(Vector2.right, vector2);
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public static Quaternion Random2DRotation()
    {
        return Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
    }

    public static Vector2 RandomVectorDeviation(this Vector2 aim, float maxDeviation)
    {
        float deviation = Random.Range(-maxDeviation, maxDeviation);
        return aim.NormalizeVectorAndRotateByDegrees(deviation);
    }

    public static Vector2 RotateVector2ByDegrees(this Vector2 vector, float angle)
    {
        return vector.NormalizeVectorAndRotateByDegrees(angle) * vector.magnitude;
    }
    public static Vector2 NormalizeVectorAndRotateByDegrees(this Vector2 vector, float angle)
    {
        angle += Vector2.SignedAngle(Vector2.right, vector);
        return Vector2FromAngle(angle);
    }

    public static Vector2 MultiplyAsImaginary(this Vector2 a, Vector2 b) //not tested
    {
        float r = a.x * b.x - a.y * b.y;
        float i = a.x * b.y + a.y * b.x;
        return new Vector2(r, i);
    }

    public static Vector2 RandomUnitVector()
    {
        return Vector2FromAngle(Random.Range(0f, 360f));
    }
    public static Vector2 Vector2FromAngle(this float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    public static Vector2 Average(Vector2 a, Vector2 b)
    {
        return (a + b) / 2f;
    }
    public static Vector2 Average(Vector2 a, Vector2 b, Vector2 c)
    {
        return (a + b + c) / 3f;
    }
    public static Vector2 Average(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        return (a + b + c + d) / 4f;
    }
    
    public static Vector2 LockPositionToGrid(this Vector2 position, float cellSize)
    {
        Vector2 cellPos = position / cellSize;
        Vector2 roundedCellPos = new Vector2(Mathf.Round(cellPos.x), Mathf.Round(cellPos.y));
        return roundedCellPos * cellSize;
    }
    
    public static Vector2 LockPositionToGridWithOrigin(this Vector2 position, float cellSize, Vector2 gridOrigin)
    {
        return gridOrigin + LockPositionToGrid(position - gridOrigin, cellSize);
    }

    public static bool Approximately(this Vector2 a, Vector2 b)
    {
        return (a - b).sqrMagnitude < 0.000004;
    }

    /// <summary>
    /// Returns the vector with the smallest magnitude.
    /// </summary>
    public static Vector2 MinMagnitude(params Vector2[] vectors)
    {
        float record = Mathf.Infinity;
        Vector2 rez = Vector2.positiveInfinity;
        foreach (Vector2 vector in vectors)
        {
            float sqrMagnitude = vector.sqrMagnitude;
            if (vector.sqrMagnitude < record)
            {
                record = sqrMagnitude;
                rez = vector;
            }
        }
        return rez;
    }
    /// <summary>
    /// Returns the vector with the largest magnitude.
    /// </summary>
    public static Vector2 Max(params Vector2[] vectors)
    {
        float record = Mathf.NegativeInfinity;
        Vector2 rez = Vector2.zero;
        foreach (Vector2 vector in vectors)
        {
            float sqrMagnitude = vector.sqrMagnitude;
            if (vector.sqrMagnitude > record)
            {
                record = sqrMagnitude;
                rez = vector;
            }
        }
        return rez;
    }

    public static Vector2 MoveLeft(this Vector2 vector, float by)
    {
        return vector + Vector2.left * by;
    }
    public static Vector2 MoveRight(this Vector2 vector, float by)
    {
        return vector + Vector2.right * by;
    }
    public static Vector2 MoveUp(this Vector2 vector, float by)
    {
        return vector + Vector2.up * by;
    }
    public static Vector2 MoveDown(this Vector2 vector, float by)
    {
        return vector + Vector2.down * by;
    }
}
