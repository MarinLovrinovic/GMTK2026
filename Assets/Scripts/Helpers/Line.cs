using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : ILine
{
    private Vector2 from;
    private Vector2 to;
    public Vector2 From => from;
    public Vector2 To => to;

    public Line(Vector2 a, Vector2 b)
    {
        from = a;
        to = b;
    }
}
