using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Swizzling
{
    public static Vector2 ox(this Vector2 a)
    {
        return new Vector2(0f, a.x);
    }
    public static Vector2 oy(this Vector2 a)
    {
        return new Vector2(0f, a.y);
    }
    public static Vector2 xo(this Vector2 a)
    {
        return new Vector2(a.x, 0f);
    }
    public static Vector2 xx(this Vector2 a)
    {
        return new Vector2(a.x, a.x);
    }
    public static Vector2 yo(this Vector2 a)
    {
        return new Vector2(a.y, 0f);
    }
    public static Vector2 yx(this Vector2 a)
    {
        return new Vector2(a.y, a.x);
    }
    public static Vector2 yy(this Vector2 a)
    {
        return new Vector2(a.y, a.y);
    }
    public static Vector3 xyo(this Vector2 a)
    {
        return new Vector3(a.x, a.y, 0f);
    }
    public static Vector3 xoy(this Vector2 a)
    {
        return new Vector3(a.x, 0f, a.y);
    }
    
    //vec2
    //ox oy
    //xo xx
    //yo yx yy
    
    //xyo xoy
    
    public static Vector3 xzy(this Vector3 a)
    {
        return new Vector3(a.x, a.z, a.y);
    }
    public static Vector3 xyo(this Vector3 a)
    {
        return new Vector3(a.x, a.y, 0f);
    }
    public static Vector3 xzo(this Vector3 a)
    {
        return new Vector3(a.x, a.z, 0f);
    }
    public static Vector3 xoy(this Vector3 a)
    {
        return new Vector3(a.x, 0f, a.y);
    }
    public static Vector3 xoz(this Vector3 a)
    {
        return new Vector3(a.x, 0f, a.z);
    }
    
    public static Vector2 xy(this Vector3 a)
    {
        return new Vector2(a.x, a.y);
    }
    public static Vector2 xz(this Vector3 a)
    {
        return new Vector2(a.x, a.z);
    }
    //vec3
    //xzy xyo xzo xoy xoz
    //xy xz
    
    public static Vector2 Append(this float x, float y)
    {
        return new Vector2(x, y);
    }
    public static Vector3 Append(this float x, Vector2 yz)
    {
        return new Vector3(x, yz.x, yz.y);
    }
    public static Vector3 Append(this Vector2 xy, float z)
    {
        return new Vector3(xy.x, xy.y, z);
    }
}