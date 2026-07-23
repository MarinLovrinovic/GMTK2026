using System;
using UnityEngine;

public static class RectHelper
{
    public static bool Contains(this Rect a, Rect b)
    {
        return a.Contains(b.min) && a.Contains(b.max);
    }

    public static Rect MinMaxRect(Vector2 min, Vector2 max)
    {
        return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }

    public static float Distance(Rect rect, Vector2 point)
    {
        float dx = Mathf.Max(rect.xMin - point.x, 0f, point.x - rect.xMax);
        float dy = Mathf.Max(rect.yMin - point.y, 0f, point.y - rect.yMax);
        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    public static Rect Average(Rect a, Rect b)
    {
        return new Rect((a.position + b.position) / 2f, (a.size + b.size) / 2f);
    }
    
    public static Rect Lerp(Rect a, Rect b, float t)
    {
        return MinMaxRect(Vector2.Lerp(a.min, b.min, t), Vector2.Lerp(a.max, b.max, t));
    }

    public static Rect LerpUnclamped(Rect a, Rect b, float t)
    {
        return MinMaxRect(Vector2.LerpUnclamped(a.min, b.min, t), Vector2.LerpUnclamped(a.max, b.max, t));
    }

    /// <summary>
    /// Use this to transform an entire rect using a point transformation.
    /// </summary>
    public static Rect TransformRect(this Rect rect, Func<Vector2, Vector2> pointTransformation)
    {
        return MinMaxRect(
            pointTransformation.Invoke(rect.min),
            pointTransformation.Invoke(rect.max));
    }
    /// <summary>
    /// Use this to transform an entire rect using a point transformation.
    /// </summary>
    public static Rect TransformRect(this Rect rect, Func<Vector2, Vector3> pointTransformation)
    {
        return MinMaxRect(
            pointTransformation.Invoke(rect.min),
            pointTransformation.Invoke(rect.max));
    }

    public static Vector2 GetBottomLeft(this Rect rect)
    {
        return rect.min;
    }
    public static Rect MoveBottomLeftTo(this Rect rect, Vector2 position)
    {
        rect.position = position;
        return rect;
    }
    
    public static Vector2 GetTopLeft(this Rect rect)
    {
        return new Vector2(rect.xMin, rect.yMax);
    }
    public static Rect MoveTopLeftTo(this Rect rect, Vector2 position)
    {
        rect.position = position + Vector2.down * rect.height;
        return rect;
    }
    
    public static Vector2 GetTopRight(this Rect rect)
    {
        return rect.max;
    }
    public static Rect MoveTopRightTo(this Rect rect, Vector2 position)
    {
        rect.position = position - rect.size;
        return rect;
    }
    
    public static Vector2 GetBottomRight(this Rect rect)
    {
        return new Vector2(rect.xMax, rect.yMin);
    }
    public static Rect MoveBottomRightTo(this Rect rect, Vector2 position)
    {
        rect.position = position + Vector2.left * rect.width;
        return rect;
    }
    
    public static Vector2 GetCenterLeft(this Rect rect)
    {
        return Vector2Helper.Average(rect.GetBottomLeft(), rect.GetTopLeft());
    }
    public static Rect MoveCenterLeftTo(this Rect rect, Vector2 position)
    {
        rect.position = position + Vector2.down * rect.height / 2f;
        return rect;
    }
    
    public static Vector2 GetTopCenter(this Rect rect)
    {
        return Vector2Helper.Average(rect.GetTopLeft(), rect.GetTopRight());
    }
    public static Rect MoveTopCenterTo(this Rect rect, Vector2 position)
    {
        rect.position = position + Vector2.down * rect.height + Vector2.left * rect.width / 2f;
        return rect;
    }
    
    public static Vector2 GetCenterRight(this Rect rect)
    {
        return Vector2Helper.Average(rect.GetBottomRight(), rect.GetTopRight());
    }
    public static Rect MoveCenterRightTo(this Rect rect, Vector2 position)
    {
        rect.position = position + Vector2.left * rect.width + Vector2.down * rect.height / 2f;
        return rect;
    }
    
    public static Vector2 GetBottomCenter(this Rect rect)
    {
        return Vector2Helper.Average(rect.GetBottomRight(), rect.GetBottomLeft());
    }
    public static Rect MoveBottomCenterTo(this Rect rect, Vector2 position)
    {
        rect.position = position + Vector2.left * rect.width / 2f;
        return rect;
    }
}
