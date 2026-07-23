using UnityEditor;
using UnityEngine;

public static class DebugHelper
{
    public static class Handles
    {
        public static void DrawRect(Rect rect, float zPosition, Color color)
        {
            Vector3[] corners = {rect.min.Append(zPosition),
                Rect.NormalizedToPoint(rect, Vector2.up).Append(zPosition),
                rect.max.Append(zPosition),
                Rect.NormalizedToPoint(rect, Vector2.right).Append(zPosition)
            };
            
            UnityEditor.Handles.DrawSolidRectangleWithOutline(corners, color, color);
        }
    }
    public static class Gizmos
    {
    }
    public static class Debug
    {
    }
}
