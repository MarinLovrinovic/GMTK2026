using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIHelper
{
    
    public static Vector3 GetWorldCenter(this RectTransform rectTransform)
    {
        return rectTransform.localToWorldMatrix.MultiplyPoint(rectTransform.rect.center);
    }

    public static bool RectTransformOverlapsRectTransform(this RectTransform a, RectTransform b)
    {
        return a.WorldSpaceRect().Overlaps(b.WorldSpaceRect());
    }
    
    public static bool RectTransformContainsRectTransform(this RectTransform a, RectTransform b)
    {
        return a.WorldSpaceRect().Contains(b.WorldSpaceRect());
    }

    public static bool RectTransformContainsPoint(this RectTransform rectTransform, Vector2 point)
    {
        return rectTransform.WorldSpaceRect().Contains(point);
    }
    
    public static Rect WorldSpaceRect(this RectTransform rectTransform)
    {
        Rect localSpaceRect = rectTransform.rect;

        Vector2 worldMin = rectTransform.localToWorldMatrix.MultiplyPoint(localSpaceRect.min);
        Vector2 worldMax = rectTransform.localToWorldMatrix.MultiplyPoint(localSpaceRect.max);

        return RectHelper.MinMaxRect(worldMin, worldMax);
    }
}
