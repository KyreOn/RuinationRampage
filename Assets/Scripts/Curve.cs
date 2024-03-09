using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public Vector3 controlPoint;

    public Curve(Vector3 a, Vector3 b, Vector3 c)
    {
        startPoint = a;
        endPoint = b;
        controlPoint = c;
    }
    
    public Vector3 Evaluate(float t)
    {
        var ac = Vector3.Lerp(startPoint,   controlPoint, t);
        var bc = Vector3.Lerp(controlPoint, endPoint,     t);
        return Vector3.Lerp(ac, bc, t);
    }
}
