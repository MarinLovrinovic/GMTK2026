
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MathHelper
{
    public static bool LinesIntersect(ILine a, ILine b)
    {
        float x1 = a.From.x;
        float y1 = a.From.y;
        float x2 = a.To.x;
        float y2 = a.To.y;
        float x3 = b.From.x;
        float y3 = b.From.y;
        float x4 = b.To.x;
        float y4 = b.To.y;

        float den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
        if (den == 0f)
        {
            return false;
        }

        float t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / den;
        float u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / den;

        if ((0f < t && t < 1f) && (0f < u && u < 1f))
        {
            return true;
        }
        return false;
    }

    public static MathHelperRaycastHit Raycast(Vector2 from, Vector2 direction, ILine against)
    {
        float x1 = against.From.x;
        float y1 = against.From.y;
        float x2 = against.To.x;
        float y2 = against.To.y;
        float x3 = from.x;
        float y3 = from.y;
        float x4 = from.x + direction.x;
        float y4 = from.y + direction.y;

        float den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
        if (den == 0f)
        {
            return new MathHelperRaycastHit(Vector2.zero, false);
        }

        float t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / den;
        float u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / den;

        if ((0f < t && t < 1f) && 0f < u)
        {
            Vector2 hitPoint = new Vector2(x1 + t * (x2 - x1), y1 + t * (y2 - y1));
            return new MathHelperRaycastHit(hitPoint, true);
        }
        return new MathHelperRaycastHit(Vector2.zero, false);
    }

    public static MathHelperRaycastHit Raycast(Vector2 from, Vector2 direction, Rect against)
    {
        float margin = 0.0000001f;
        Line[] sides =
        {
            new Line(against.GetBottomLeft().MoveDown(margin), against.GetTopLeft().MoveUp(margin)),
            new Line(against.GetTopLeft().MoveLeft(margin), against.GetTopRight().MoveRight(margin)),
            new Line(against.GetTopRight().MoveUp(margin), against.GetBottomRight().MoveDown(margin)),
            new Line(against.GetBottomRight().MoveRight(margin), against.GetBottomLeft().MoveLeft(margin))
        };

        float closestHit = Mathf.Infinity;
        Vector2 rezPoint = Vector2.zero;
        bool hitSomething = false;
        foreach (Line side in sides)
        {
            MathHelperRaycastHit hit = Raycast(from, direction, side);
            if (hit)
            {
                hitSomething = true;
                float distance = Vector2.Distance(from, hit.point);
                if (distance < closestHit)
                {
                    closestHit = distance;
                    rezPoint = hit.point;
                }
            }
        }
        return new MathHelperRaycastHit(rezPoint, hitSomething);
    }

    public static float MinimumDistanceOfMovingPoints(Vector2 r1, Vector2 v1, Vector2 r2, Vector2 v2)
    {
        float c1 = r1.x - r2.x;
        float c2 = v1.x - v2.x;
        float c3 = r1.y - r2.y;
        float c4 = v1.y - v2.y;

        float t = (-c1 * c2 - c3 * c4) / (c2 * c2 + c4 * c4);

        return Vector2.Distance(r1 + t * v1, r2 + t * v2);
    }

    public static bool MovingCirclesIntersect(Vector2 pos1, Vector2 v1, float r1, Vector2 pos2, Vector2 v2, float r2)
    {
        return MinimumDistanceOfMovingPoints(pos1, v1, pos2, v2) <= r1 + r2;
    }

    //returns the time of collision
    public static float MovingCirclesCollisionTime(Vector2 pos1, Vector2 v1, float r1, Vector2 pos2, Vector2 v2, float r2)
    {
        float c1 = pos1.x - pos2.x;
        float c2 = v1.x - v2.x;
        float c3 = pos1.y - pos2.y;
        float c4 = v1.y - v2.y;

        float A = c2 * c2 + c4 * c4;
        float B = 2 * c1 * c2 + 2 * c3 * c4;
        float C = c1 * c1 + c3 * c3 - Mathf.Pow(r1 + r2, 2);

        float det = B * B - 4 * A * C;
        if (det < 0) return Mathf.Infinity; // indicating that they did not collide
        
        det = Mathf.Sqrt(det);

        float t1 = (-B + det) / (2 * A);
        float t2 = (-B - det) / (2 * A);
        float t = Mathf.Min(t1, t2);

        if (t < 0f) return Mathf.Infinity; //they would have collided in the past

        
        return t;
    }
    
    public static float RandomFromDistribution(this AnimationCurve distribution)
    {
        if (distribution.length < 2) return 0f;
        
        int resolution = 10;
        
        float min = distribution.keys[0].time;
        float max = distribution.keys[distribution.length - 1].time;

        float range = max - min;
        float step = range / resolution;
        
        float[] slices = new float[resolution];

        float area = 0f;
        for (int i = 0; i < resolution; i++)
        {
            slices[i] = distribution.Evaluate(min + 0.5f * step + i * step);
            area += slices[i];
        }
        
        float random = Random.Range(0f, area);

        int index = 0;
        for (int i = 0; i < resolution; i++)
        {
            random -= slices[i];
            if (random < 0f)
            {
                index = i;
                break;
            }
        }
        
        return Random.Range(min + index * step, min + index * step + step);
    }

    public static bool RandomBool(float probabilityOfTrue)
    {
        if (probabilityOfTrue == 0f)
            return false;
        return Random.value <= probabilityOfTrue;
    }

    public static T WeightedRandomFromDistributionArray<T>(T[] items, float[] weights)
    {
        float totalWeight = 0.0f;
        foreach (float weight in weights) totalWeight += weight;

        float randomValueInWeights = Random.Range(0.0f, totalWeight);

        float cumulativeWeight = 0.0f;
        for (int i = 0; i < items.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValueInWeights < cumulativeWeight) return items[i];
        }

        return items[items.Length - 1];
    }
}

public struct MathHelperRaycastHit
{
    public readonly Vector2 point;
    private readonly bool hit;

    public MathHelperRaycastHit(Vector2 position, bool hitAnything)
    {
        point = position;
        hit = hitAnything;
    }

    public static implicit operator bool(MathHelperRaycastHit hit)
    {
        return hit.hit;
    }
}